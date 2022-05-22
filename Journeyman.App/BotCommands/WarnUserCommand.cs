using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Journeyman.App.BotCommands
{
    class WarnUserCommand : IBotCommand
    {
        private readonly IBot _bot;
        public WarnUserCommand(IBot bot)
        {
            _bot = bot;
        }

        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            var msg = update.Message?.ReplyToMessage;

            if (msg is null)
                return;

            var admin = await client.GetChatMemberAsync(update.Message.Chat.Id, update.Message.From.Id);

            if (admin.Status != ChatMemberStatus.Administrator && admin.Status != ChatMemberStatus.Creator)
                return;

            await _bot.WarnChatUserAsync(msg.From, msg.Chat.Id, msg.MessageId);
        }
    }
}
