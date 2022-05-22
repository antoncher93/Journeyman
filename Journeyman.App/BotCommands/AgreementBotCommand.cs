using Journeyman.App.Data;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.App.BotCommands
{
    class AgreementBotCommand : IBotCommand
    {
        private readonly IBot _bot;
        public AgreementBotCommand(IBot bot, BotDbContext db)
        {
            _bot = bot;
        }

        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            var replyMsg = update.Message.ReplyToMessage;

            if(replyMsg != null)
            {
                await _bot.AddChatAgreementAsync(update.Message.Chat, replyMsg.Text);
            }
        }
    }
}
