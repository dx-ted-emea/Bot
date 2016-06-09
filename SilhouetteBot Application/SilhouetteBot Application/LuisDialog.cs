using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace SilhouetteBot_Application
{

    [LuisModel("6d2b0ad4-c873-452d-92e6-cc609773c44d", "c336da5a128847388d87fc6f2f4c5f0a")]
    [Serializable]
    class SilhouetteDialog : LuisDialog<object>
    {

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = "Sorry I did not understand";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("greeting")]
        public async Task Greet(IDialogContext context, LuisResult result)
        {
            string message = "Hi, what can I do for you?";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("get")]
        public async Task Get(IDialogContext context, LuisResult result)
        {
            string deviceId = string.Empty;
            string state = string.Empty;

            foreach (var entity in result.Entities)
            {
                if (entity.Type == "deviceId")
                {
                    deviceId = entity.Entity;
                }
            }

            if (deviceId != string.Empty)
            {
                state = await GetDeviceState(deviceId);


                if (state != string.Empty)
                {
                    await context.PostAsync(state);
                }
                else
                {
                    await context.PostAsync("Device not found");
                }
            }
            else
            {
                await context.PostAsync("What is the device id?");
                
            }
            context.Wait(MessageReceived);
        }

        //public async Task MessageReceived(IDialogContext context, IAwaitable<Microsoft.Bot.Connector.Message> result)
        //{
        //    await context.PostAsync(context.ConversationData.ToString());
        //    context.Wait(MessageReceived);
        //}


        [LuisIntent("set")]
        public async Task Set(IDialogContext context, LuisResult result)
        {

            string deviceId = string.Empty;
            string state = string.Empty;

            foreach (var entity in result.Entities)
            {
                if (entity.Type == "deviceId")
                    deviceId = entity.Entity;
                else if (entity.Type == "state")
                    state = entity.Entity;
            }

            await context.PostAsync(string.Format("device {0} has state {1}", deviceId, state) );
            context.Wait(MessageReceived);
        }

        public async Task<string> GetDeviceState(string deviceId)
        {
            using (var client = new HttpClient())
            {
                string uri = "http://silhouettecluster.westeurope.cloudapp.azure.com:9013/devices/" +deviceId;
                HttpResponseMessage msg = await client.GetAsync(uri);

                if (msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();

                    return jsonResponse;
                }
                else
                    return "";
            }
           
        }
    }
}