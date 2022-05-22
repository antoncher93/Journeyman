using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Journeyman.App.BotCommands
{
    class BanUserCommand : IBotCommand
    {
        private readonly IBot _bot;
        public BanUserCommand(IBot bot)
        {
            _bot = bot;
        }

        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            var replyMsg = update.Message?.ReplyToMessage;

            if(replyMsg is null)
                return;

            var admin = await client.GetChatMemberAsync(update.Message.Chat.Id, update.Message.From.Id);

            if(admin.Status != ChatMemberStatus.Administrator && admin.Status != ChatMemberStatus.Creator)
                return;

            await _bot.RestrictUserInChatAsync(replyMsg.From, replyMsg.Chat.Id, replyMsg.MessageId);

        }
    }
}
