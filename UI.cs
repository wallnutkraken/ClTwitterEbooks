using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using TweetSharp;

namespace Lucas_Ebooks
{
    static class UI
    {

        public static void StartUI()
        {
            if (Properties.Settings.Default.FirstRun)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("BEFORE YOU BEGIN: ");
                Console.ResetColor();
                Console.WriteLine("Please read the documentation that came with this Twitter bot. " + 
                    "It contains important information. If you do not read it before setting up " + 
                    "this bot, you may run into problems while setting it up. Or you might not." +
                    "Please press any key to continue");

                Console.ReadKey(true);
                SetAPIKey();
                SetUser();
                ArchiveLocation();
                Archive.ReadArchive();
                OtherSettings();

                Console.Clear();
                Console.WriteLine("We will now parse your Twitter archive and feed it to the Markov monster.");
                Console.WriteLine("This may take a few minutes. Make yourself at home.");
                Archive.FeedAll();
                Console.Beep();
                Console.WriteLine("\nThe Markov is happy! Tweet feeding complete.");
                Console.WriteLine("Press any key to start the bot!");
                Console.ReadKey(true);
                Properties.Settings.Default.FirstRun = false;
                //Properties.Settings.Default.Save();
            }
            else
            {
                char[] validChoices = { '1', '2' };
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("\n1. Start the bot");
                Console.WriteLine("2. Reset settings");

                char selection = '\0';
                do
                {
                    selection = Console.ReadKey(true).KeyChar;
                } while (validChoices.Contains(selection) == false);

                if (selection == '1')
                {
                    /* Bot start! */
                }
                else if (selection == '2')
                {
                    ResetSettings();
                }
            }
        }

        private static void ResetSettings()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Are you SURE you want to reset settings to defaults? [Y/N]");
            Console.ResetColor();

            char selection = '\0';
            do
            {
                selection = char.ToLower(Console.ReadKey(true).KeyChar);
            } while (selection != 'y' && selection != 'n');

            if (selection == 'y')
            {
                Properties.Settings.Default.Reset();
                Console.Clear();
                Console.WriteLine("Settings reset.");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }
        private static void SetAPIKey()
        {
            char selection = 'b';
            int consX, consY;
            consX = Console.CursorLeft;
            consY = Console.CursorTop;

            Console.WriteLine("Would you like to use a custom API key? [Y/N]");
            do
            {
                selection = char.ToLower(Console.ReadKey(true).KeyChar);
            }
            while (selection != 'y' && selection != 'n');

            if (selection == 'y')
            {
                string consumerKey = "";
                string consumerSecret = "";
                do
                {
                    Console.Clear();
                    Console.WriteLine("Please enter the consumer key");
                    consumerKey = Console.ReadLine();
                    Console.WriteLine("Please enter the consumer secret");
                    consumerSecret = Console.ReadLine();

                    Console.WriteLine("\nPlease double check what you put in. Press Y to confirm and N to set it again");
                    do
                    {
                        selection = char.ToLower(Console.ReadKey(true).KeyChar);
                    } while (selection != 'y' && selection != 'n');
                } while (selection != 'y');
                Properties.Settings.Default.ConsumerKey = consumerKey;
                Properties.Settings.Default.ConsumerSecret = consumerSecret;
            }
        }

        private static void OtherSettings()
        {
            Console.Clear();
            Console.WriteLine("Do you want to any optional settings? [Y/N]");
            char selection = '\0';
            do
            {
                selection = char.ToLower(Console.ReadKey(true).KeyChar);
            } while (selection != 'y' && selection != 'n');
            if (selection == 'y')
            {
                Console.Clear();
                Console.WriteLine("Do you want the bot to reply to mentions? [Y/N]");
                do
                {
                    selection = char.ToLower(Console.ReadKey(true).KeyChar);
                } while (selection != 'y' && selection != 'n');
                if (selection == 'y')
                {
                    Properties.Settings.Default.ReplyEbook = true;
                }
                else
                {
                    Properties.Settings.Default.ReplyEbook = false;
                }
                
                int refreshRate = 300;
                do
                {
                    Console.WriteLine("How often do you want the bot to look for new tweets? Default is 300 seconds.");
                    try
                    {
                        refreshRate = int.Parse(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        refreshRate = 0;
                    }
                } while (refreshRate == 0);
                Properties.Settings.Default.TwitterFetchTimeout = refreshRate;

                int saveRate = 3600;
                do
                {
                    Console.WriteLine("How often do you want the bot to save the Markov Chain state to file?");
                    try
                    {
                        saveRate = int.Parse(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        saveRate = 0;
                    }
                } while (saveRate < 15);
                Properties.Settings.Default.SavePeriod = saveRate;

                int charLength = 140;
                do
                {
                    Console.WriteLine("How long do you want the maximum tweet length to be? (In characters)");
                    Console.WriteLine("Please do note that tweets cannot be longer than 140 characters");
                    try
                    {
                        charLength = int.Parse(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        charLength = int.MaxValue;
                    }
                } while (charLength > 140);
                Properties.Settings.Default.MaxCharacterLength = charLength;

                //Properties.Settings.Default.Save();
            }
        }

        private static void SetUser()
        {
            if (Twitter.Account == null)
            {
                Twitter.Start();
            }
            OAuthAccessToken userKey = new OAuthAccessToken();
            OAuthRequestToken requestToken = Twitter.Account.GetRequestToken();
            Uri uri = Twitter.Account.GetAuthorizationUri(requestToken);
            Process.Start(uri.ToString());

            string verifier = null;
            OAuthAccessToken user;
            do
            {
                Console.Write("Please input the authentication number: ");
                verifier = Console.ReadLine();
                user = Twitter.Account.GetAccessToken(requestToken, verifier);
                if (Twitter.Account.Response.Error != null)
                {
                    Console.WriteLine(Twitter.Account.Response.Error.Message);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey(true);
                    verifier = null;
                }
            } while (verifier == null);

            Properties.Settings.Default.UserId = user.UserId;
            Properties.Settings.Default.UserKey = user.Token;
            Properties.Settings.Default.UserSecret = user.TokenSecret;
        }

        private static void ArchiveLocation()
        {
            Console.Clear();

            Console.WriteLine("The default path to look for the twitter archive .js files is" + 
                "the 'twitter' directory where this executable is located.");
            Console.WriteLine("For more information on this, please read INSTALL.md");
            Console.WriteLine("Do you want to change the path? [Y/N]");
            char selection = '\0';
            do
            {
                selection = char.ToLower(Console.ReadKey(true).KeyChar);
            } while (selection != 'y' && selection != 'n');
            
            if (selection == 'y')
            {
                ChangePath();
            }
        }

        private static void ChangePath()
        {
            Console.Clear();
            char selection = '\0';

            string path;
            do
            {
                Console.Clear();
                Console.WriteLine("Please enter the new path for where the program should look for" +
                    "the twitter archive .js files");
                path = Console.ReadLine();
                Console.WriteLine("Double check the path. Is this correct? [Y/N]");
                do
                {
                    selection = char.ToLower(Console.ReadKey(true).KeyChar);
                } while (selection != 'y' && selection != 'n');
            } while (selection != 'y');
            Properties.Settings.Default.ArchivePath = path;
        }

    }
}
