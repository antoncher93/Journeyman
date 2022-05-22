using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.App.Handlers
{
    public class NewChatMembersHandler : UpdateHandler
    {
        private readonly IBot _bot;

        public NewChatMembersHandler(IBot bot)
        {
            _bot = bot;
        }

        public override async Task HandleAsync(ITelegramBotClient client, Update update)
        {
            var msg = update.Message;

            if (msg != null && msg.Type == Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded)
            {
                await _bot.WelcomeChatMembers(msg.Chat.Id, msg.NewChatMembers);
            }
            else await base.HandleAsync(client, update);
        }
    }
}
