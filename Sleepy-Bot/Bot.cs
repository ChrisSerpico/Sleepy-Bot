using DiscordSharp;
using DiscordSharp.Events;
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
        public static string version = "0.3c";

        // the discordclient being used
        private static DiscordClient client;

        static void Main(string[] args)
        {
            // read variables from file, this is to keep various authentication tokens secret
            Console.WriteLine("Reading variables from secrets.txt...");
            string[] variables = System.IO.File.ReadAllLines("secrets.txt");
            client = new DiscordClient(variables[0], isbot);
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

                    // if there's only one message, this is probably a simple request 
                    if (message.Length == 1)
                    {
                        HandleSimpleRequest(message[0], e);
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

        private static void HandleSimpleRequest(string message, DiscordMessageEventArgs e)
        {
            switch(message)
            {
                case "!help":
                    {
                        // !help
                        // gives a list of commands
                        e.Channel.SendMessage("I can respond to these commands (remember to put an ! first):" + 
                            "\ninfo" +
                            "\nkappa" +
                            "\nsneaky" +
                            "\nkrey" +
                            "\ntriggered");
                        break;
                    }
                case "!info":
                    {
                        // !info
                        // Gives a little bit of information about the bot
                        e.Channel.SendMessage("Hi there! I'm sleepy-bot. I was made by Chris Serpico using DiscordSharp by LuigiFan with some help from NaamloosDT.");
                        e.Channel.SendMessage("I'm currently running version " + version + "! :grinning:");
                        break;
                    }
                case "!meme":
                    {
                        // !meme
                        // sends a random file from a specific folder
                        // TODO limit this to image files 
                        Random rand = new Random();
                        string[] files = Directory.GetFiles("memes");
                        client.AttachFile(e.Channel, "me_irl", (files[rand.Next(files.Length)]));
                        break;
                    }

                // twitch chat emotes
                case "!kappa":
                    {
                        client.AttachFile(e.Channel, " ", "memes/kappa.jpg");
                        break;
                    }
                case "!sneaky":
                case "!sneakygasm":
                    {
                        client.AttachFile(e.Channel, " ", "memes/sneaky.jpg");
                        break;
                    }
                case "!krey":
                case "!kreygasm":
                    {
                        client.AttachFile(e.Channel, " ", "memes/krey.jpg");
                        break;
                    }
                case "!kappapride":
                    {
                        client.AttachFile(e.Channel, " ", "memes/kappapride.jpg");
                        break;
                    }

                case "!triggered":
                    {
                        // !triggered
                        // for anyone reading this code, this was a request. :/


                        e.Channel.SendMessage("Ṭ̷Ř̥̤̤̻̥̥ͧ̏ͦ̋͑͡Ɨ̘͉̲̯̹͔̿ͯͦ͋͂͡Ǥ̸̷͈͇͉̟̫͚͖͉̼̰̱̩͔̙̖̱̌͑ͥ̐ͤͧ̂͌̃ͬ͟͜ͅĠ̟͓͇̺̭̮̇̄̍̃ͬͣ͂ͪ̽̃̀͜Ɇ̛ͦ̄̓ͪ̇̌̄̒̊̓̾̐͒͋ͭ̀͗̚͝҉̧͙͍̦̣̤͇͓͙̲͍̪̤̻͢ͅṜ͓̠̘̥̼̈́̌ͬ͜ͅḚ̬̯͎͉̙̉ͧ͆̕Ƌ̶");
                        break;
                    }
            }
        }
    }
}
