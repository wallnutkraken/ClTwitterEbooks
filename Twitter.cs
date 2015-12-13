using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TweetSharp;

namespace ClTwitter_Ebooks
{
    class Twitter
    {
        public static TwitterService Account { get; set; }


        public static void Start()
        {
            Account = new TwitterService(Properties.Settings.Default.ConsumerKey, Properties.Settings.Default.ConsumerSecret);
            Twitter.Account.AuthenticateWith(Properties.Settings.Default.ConsumerKey, Properties.Settings.Default.ConsumerSecret);
        }

        public static void BotStart()
        {

        }
    }
}
