using Journeyman.App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Journeyman.App
{
    public static class Extensions
    {
        public static string CreateMention(this Telegram.Bot.Types.User user)
        {
            string mention = user.Username ?? user.FirstName + " " + user.LastName;
            return $"[{mention}](tg://user?{user.Id})";
        }
    }
}
