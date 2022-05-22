using Journeyman.App.BotCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Journeyman.App.Handlers
{
    public class BotCommandHandler : UpdateHandler
    {
        private readonly IDictionary<string, Type> _commandDictionary = new Dictionary<string, Type>();
        private readonly IServiceProvider _serviceProvider;

        public BotCommandHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _commandDictionary.Add("/warn", typeof(WarnUserCommand));
            _commandDictionary.Add("/ban", typeof(BanUserCommand));
            _commandDictionary.Add("/start", typeof(StartBotCommand));
            _commandDictionary.Add("/agreement", typeof(AgreementBotCommand));
        }

        public override async Task HandleAsync(ITelegramBotClient client, Update update)
        {
            var msg = update.Message;

            if (msg?.Entities != null && msg.Entities.Length > 0)
            {
                for (int i = 0; i < msg.Entities.Length; i++)
                {
                    if (msg.Entities[i].Type == MessageEntityType.BotCommand)
                    {
                        var commandName = msg.EntityValues.ElementAt(i);
                        if(_commandDictionary.ContainsKey(commandName))
                        {
                            var commandType = _commandDictionary[commandName];
                            var command = (IBotCommand)_serviceProvider.GetService(commandType);
                            await command.ExecuteAsync(client, update);
                            await client.DeleteMessageAsync(msg.Chat, msg.MessageId);
                        }
                        
                    }
                    else await base.HandleAsync(client, update);
                }
            }
            else await base.HandleAsync(client, update);
        }
    }
}
