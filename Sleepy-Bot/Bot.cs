using DiscordSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleepy_Bot
{
    class Bot
    {
        // whether this bot is using the API (it is currently)
        public static bool isbot = true;

        // the current version
        public static string version = "0.2";

        static void Main(string[] args)
        {
            // read variables from file, this is to keep various authentication tokens secret
            Console.WriteLine("Reading variables from secrets.txt...");
            string[] variables = System.IO.File.ReadAllLines("secrets.txt");
            DiscordClient client = new DiscordClient(variables[0], isbot);
            client.ClientPrivateInformation.Email = variables[1];
            client.ClientPrivateInformation.Password = variables[2];

            
            // Now define possible events 
            // This is how sleepy-bot interacts with users
            Console.WriteLine("Defining events...");
            
            // sleepy-bot can recieve messages
            client.MessageReceived += (sender, e) => 
                {
                    // split the input message based on spaces
                    // the value at 0 should be the command 
                    // TODO make this a switch case in a different function somewhere else 
                    string[] message = e.MessageText.ToLower().Split(' ');

                    // !help
                    // gives a list of commands
                    if (message[0] == "!help")
                    {
                        e.Channel.SendMessage("I can respond to these commands (remember to put an ! first):");
                        e.Channel.SendMessage("info");
                    }

                    // !info
                    else if (message[0] == "!info")
                    {
                        e.Channel.SendMessage("Hi there! I'm sleepy-bot. I was made by Chris Serpico using DiscordSharp by LuigiFan with some help from NaamloosDT.");
                        e.Channel.SendMessage("I'm currently running version " + version + "! :grinning:");
                    }

                    // !meme
                    else if (message[0] == "!meme")
                    {
                        Random rand = new Random();
                        string[] files = Directory.GetFiles("memes");
                        client.AttachFile(e.Channel, "me_irl", (files[rand.Next(files.Length)]));
                        
                    }

                    // sleepy bot doesn't like being insulted
                    else if (message.Contains<string>("hate") && message.Contains<string>("sleepy-bot"))
                    {
                        e.Channel.SendMessage("That's not a very nice thing to say :cry:");
                    }
                };

            // sleepy-bot can also receive private messages, although he can't really do anything with them right now
            client.PrivateMessageReceived += (sender, e) =>
            {
                e.Author.SendMessage("Hi, sorry, I can't really understand DMs right now. Maybe in the future?");
            };

            // Finally, attempt to connect to discord
            try
            {
                Console.WriteLine("Sending login request...");
                client.SendLoginRequest();
                client.Connect();
                Console.WriteLine("Client connected!");

                // we're successfully connected! Now the bot is running
                Console.WriteLine("Bot is running. Press any key to exit.");
            }
            catch(Exception e)
            {
                // something has gone wrong
                Console.WriteLine("Something went wrong while trying to connect\n" + e.Message + "\nPress any key to close console.");
            }

            
            // wait for button press to exit
            Console.ReadKey();
            client.Logout();
            Environment.Exit(0); 
        }
    }
}
