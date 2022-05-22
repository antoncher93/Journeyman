using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.App.BotCommands
{
    class StartBotCommand : IBotCommand
    {
        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            var msg = update.Message;
            await client.SendTextMessageAsync(msg.Chat.Id, "Я в работе",
                replyToMessageId: msg.MessageId);
        }
    }
}
