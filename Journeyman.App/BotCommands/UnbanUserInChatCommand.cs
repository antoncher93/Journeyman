using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.App.BotCommands
{
    class UnbanUserInChatCommand : IBotCommand
    {
        public UnbanUserInChatCommand()
        {
        }

        public async Task ExecuteAsync(ITelegramBotClient client, Update update)
        {
            var permissions = new ChatPermissions()
            {
                CanAddWebPagePreviews = true,
                CanChangeInfo = true,
                CanInviteUsers = true,
                CanPinMessages = true,
                CanSendMediaMessages = true,
                CanSendMessages = true,
                CanSendOtherMessages = true,
                CanSendPolls = true
            };

            var from = update.Message.From;
            var replyToMsg = update.Message.ReplyToMessage;

            if (replyToMsg is null)
                return;

            await client.UnbanChatMemberAsync(replyToMsg.Chat, replyToMsg.From.Id);
        }
    }
}
