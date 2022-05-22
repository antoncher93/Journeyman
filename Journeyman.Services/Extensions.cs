using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddJourneyman(this IServiceCollection services, string token)
        {
            //services.AddTransient<ITelegramBotClient>(p => new TelegramBotClient(token));
            //services.AddScoped<IBot, Bot>();
            return services;
        }
    }
}
