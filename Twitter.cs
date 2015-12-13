using System;
using System.Collections.Generic;
using ClutteredMarkov;
using System.Threading;
using TweetSharp;
using System.Linq;

namespace ClTwitter_Ebooks
{
    class Twitter
    {
        public static TwitterService Account { get; set; }

        /// <summary>
        /// Starts up the TwitterService object
        /// </summary>
        public static void Start()
        {
            Account = new TwitterService(Properties.Settings.Default.ConsumerKey, Properties.Settings.Default.ConsumerSecret);
        }

        /// <summary>
        /// Logs into twitter using the credentials from settings
        /// </summary>
        public static void LoginTwitter()
        {
            Account.AuthenticateWith(Properties.Settings.Default.UserKey, Properties.Settings.Default.UserSecret);
            if (Account.Response.Error != null)
            {
                Console.WriteLine(Account.Response.Error.Code + ": " + Account.Response.Error.Message);
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Removes any @mentions from a string
        /// </summary>
        /// <param name="original">The string to rid of mentions</param>
        private static string RemoveMentions(string original)
        {
            char[] splitters = { ' ' };
            string[] words = original.Split(splitters);
            string newString = "";
            foreach (string word in words)
            {
                if (word.StartsWith("@") == false)
                {
                    newString += word;
                }
            }

            return newString;
        }

        /// <summary>
        /// A Timer(object) method to get new tweets from twitter and feed them to the Markov chain
        /// </summary>
        private static void RefreshTweets(object stateinfo)
        {
            ListTweetsOnUserTimelineOptions twOpts = new ListTweetsOnUserTimelineOptions();
            twOpts.IncludeRts = false;
            if (Properties.Settings.Default.LastTweetId != 0)
            {
                twOpts.SinceId = Properties.Settings.Default.LastTweetId;
            }
            twOpts.UserId = Properties.Settings.Default.UserId;
            List<TwitterStatus> tweets = Account.ListTweetsOnUserTimeline(twOpts).ToList();
            if (tweets.Count != 0)
            {
                foreach (TwitterStatus tweet in tweets)
                {
                    if (tweet.Id >= Properties.Settings.Default.LastTweetId)
                    {
                        Markov.Feed(RemoveMentions(tweet.Text));
                    }
                }
                Properties.Settings.Default.LastTweetId = tweets[tweets.Count - 1].Id;
            }

            if (Properties.Settings.Default.ReplyEbook)
            {
                ListTweetsMentioningMeOptions mentionOpts = new ListTweetsMentioningMeOptions();
                if (Properties.Settings.Default.LastMentionId != 0)
                {
                    mentionOpts.SinceId = Properties.Settings.Default.LastMentionId;
                }
                List<TwitterStatus> mentions = Account.ListTweetsMentioningMe(mentionOpts).ToList();
                if (Properties.Settings.Default.LastMentionId == 0)
                {
                    if (mentions.Count != 0)
                    {
                        Properties.Settings.Default.LastMentionId = mentions.Last().Id;
                    }
                    return;
                }
                if (mentions.Count != 0)
                {
                    foreach (TwitterStatus tweet in mentions)
                    {
                        SendTweetOptions tweetOpts = new SendTweetOptions();
                        int length = Properties.Settings.Default.MaxCharacterLength - tweet.Author.ScreenName.Length - 2;
                        tweetOpts.Status = "@" + tweet.Author.ScreenName + " " + MarkovGenerator.Create(length);
                        tweetOpts.InReplyToStatusId = tweet.Id;
                        
                        Account.SendTweet(tweetOpts);
                    }
                    Properties.Settings.Default.LastMentionId = mentions[mentions.Count - 1].Id;
                }
            }
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Posts a tweet with a Markov-chain generated sentence. Timer method.
        /// </summary>
        private static void PostEbooks(object state)
        {
            SendTweetOptions tweetOpts = new SendTweetOptions();
            tweetOpts.Status = MarkovGenerator.Create(Properties.Settings.Default.MaxCharacterLength);
            Account.SendTweet(tweetOpts);

            if (Account.Response.Error != null)
            {
                Console.Beep();
                Console.WriteLine(Account.Response.Error.Code + ": " + Account.Response.Error.Message);
            }
        }

        /// <summary>
        /// Saves the bots memory. Timer method.
        /// </summary>
        private static void SaveBotMemory(object state)
        {
            Markov.Save();
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Starts the bot and creates the threads for updating
        /// </summary>
        public static void BotStart()
        {
            if (Account == null)
            {
                Start();
                LoginTwitter();
            }
            Markov.Load();

            TimerCallback refreshCall = RefreshTweets;
            int refreshtime = Properties.Settings.Default.TwitterFetchTimeout * 1000;
            Timer refresh = new Timer(refreshCall, null, 0, refreshtime);

            TimerCallback saveCall = SaveBotMemory;
            int savetime = Properties.Settings.Default.SavePeriod * 1000;
            Timer save = new Timer(refreshCall, null, 0, savetime);

            TimerCallback postCall = PostEbooks;
            int posttime = Properties.Settings.Default.PostRate * 60000;
            Timer post = new Timer(postCall, null, 0, posttime);

            Console.WriteLine("Bot started. You may press Q at any time to stop the application.");
            char selection;
            do
            {
                selection = char.ToLower(Console.ReadKey(true).KeyChar);
            } while (selection != 'q');
        }
    }
}
