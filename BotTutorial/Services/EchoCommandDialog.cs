using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BotTutorial.Services
{
    [Serializable]
    public class EchoCommandDialog
    {
        public static readonly CommandDialog<object> dialog = new CommandDialog<object>().On<bool>(new Regex("^reset"), async (context, msg) =>
        {
            PromptDialog.Confirm(context, dialog.ResultHandler,
            "Are you sure you want to reset the count?",
            "Didn't get that!");
        }, async (context, result) =>
        {
            var confirm = await result;
            if (confirm)
            {
                context.UserData.SetValue("count", 0);
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
        }).OnDefault<object>(async (context, msg) =>
        {
            int count;
            var message = await msg;
            string fullMsg;
            context.UserData.TryGetValue("count", out count);
            bool res = context.UserData.TryGetValue("fullMsg", out fullMsg);
            if (!res)
            {
                fullMsg = message.Text;
            }
            else
            {
                fullMsg += message.Text;
            }
            context.UserData.SetValue("fullMsg", fullMsg);
            context.UserData.SetValue("count", ++count);
            //await context.PostAsync(string.Format("{0}: You said {1}", count, message.Text));
            await context.PostAsync(string.Format("{0}: You said bla bla {1}", count, fullMsg));
            context.Wait(dialog.MessageReceived);
        });

    }
}