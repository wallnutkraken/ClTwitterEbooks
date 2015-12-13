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


        public static void Start()
        {
            Account = new TwitterService(Properties.Settings.Default.ConsumerKey, Properties.Settings.Default.ConsumerSecret);
        }

        private static void LoginTwitter()
        {
            Account.AuthenticateWith(Properties.Settings.Default.UserKey, Properties.Settings.Default.UserSecret);
            if (Account.Response.Error != null)
            {
                Console.WriteLine(Account.Response.Error.Code + ": " + Account.Response.Error.Message);
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

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

        private static void SaveBotMemory(object state)
        {
            Markov.Save();
            Properties.Settings.Default.Save();
        }

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

            Console.WriteLine("Bot started. You may press Q at any time to stop the application.");
            char selection;
            do
            {
                selection = char.ToLower(Console.ReadKey(true).KeyChar);
            } while (selection != 'q');
        }
    }
}
