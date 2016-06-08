using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace SilhouetteBot_Application
{

    [LuisModel("6d2b0ad4-c873-452d-92e6-cc609773c44d", "c336da5a128847388d87fc6f2f4c5f0a")]
    [Serializable]
    class SilhouetteDialog : LuisDialog<object>
    {

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("get")]
        public async Task Get(IDialogContext context, LuisResult result)
        {
            
            await context.PostAsync(result.Entities[0].Type);

            context.Wait(MessageReceived);
        }

        //        private readonly Dictionary<string, Alarm> alarmByWhat = new Dictionary<string, Alarm>();

        //        public const string DefaultAlarmWhat = "default";

        //        public bool TryFindAlarm(LuisResult result, out Alarm alarm)
        //        {
        //            alarm = null;

        //            string what;

        //            EntityRecommendation title;
        //            if (result.TryFindEntity(Entity_Alarm_Title, out title))
        //            {
        //                what = title.Entity;
        //            }
        //            else
        //            {
        //                what = DefaultAlarmWhat;
        //            }

        //            return this.alarmByWhat.TryGetValue(what, out alarm);
        //        }

        //        public const string Entity_Alarm_Title = "builtin.alarm.title";
        //        public const string Entity_Alarm_Start_Time = "builtin.alarm.start_time";
        //        public const string Entity_Alarm_Start_Date = "builtin.alarm.start_date";





        //        [LuisIntent("set")]
        //        public async Task Set(IDialogContext context, LuisResult result)
        //        {
        //            Alarm alarm;
        //            if (TryFindAlarm(result, out alarm))
        //            {
        //                await context.PostAsync($"found alarm {alarm}");
        //            }
        //            else
        //            {
        //                await context.PostAsync("did not find alarm");
        //            }

        //            context.Wait(MessageReceived);
        //        }


        //        [Serializable]
        //        public sealed class Alarm : IEquatable<Alarm>
        //        {
        //            public DateTime When { get; set; }
        //            public string What { get; set; }

        //            public override string ToString()
        //            {
        //                return $"[{this.What} at {this.When}]";
        //            }

        //            public bool Equals(Alarm other)
        //            {
        //                return other != null
        //                    && this.When == other.When
        //                    && this.What == other.What;
        //            }

        //            public override bool Equals(object other)
        //            {
        //                return Equals(other as Alarm);
        //            }

        //            public override int GetHashCode()
        //            {
        //                return this.What.GetHashCode();
        //            }
        //        }
        //    }
        //}

    }
}