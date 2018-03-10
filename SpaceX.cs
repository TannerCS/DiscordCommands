//Makes use of https://api.spacexdata.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;

namespace DiscordBot
{
    [Group("spacex"), Summary("Gets info about SpaceX")]
    public class SpaceX : ModuleBase
    {
        [Command("latest"), Summary("Gets the latest launch from SpaceX")]
        public async Task Latest()
        {
            //Create a webclient and download the JSON from the SpaceX API.
            WebClient client = new WebClient();
            string JSON = client.DownloadString("https://api.spacexdata.com/v2/launches/latest");
            //Parse the string into usable data.
            JObject data = JObject.Parse(JSON);
            //Create an embedBuilder
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Latest Launch");//create a title with "Latest Launch"
            builder.WithDescription(data["details"].ToString());
            builder.WithColor(Color.Blue); //Set the Embed to blue
            builder.WithThumbnailUrl(data["links"]["mission_patch"].ToString());
            builder.AddInlineField("Launch Success", data["launch_success"].ToString()); //Gets the JSON data and returns a true or false string
            builder.AddInlineField("Rocket", data["rocket"]["second_stage"]["payloads"][0]["payload_id"].ToString() + " (" + data["rocket"]["rocket_name"].ToString() + ")"); //Gets the rocket payload and rocket name

            await ReplyAsync("", false, builder); //Send the message.
        }

        //What is being ran when typing !spacex
        public async Task Info()
        {
            await ReplyAsync("Type !spacex latest for latest info about SpaceX");
        }
    }
}
