using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Journeyman.App.Handlers
{
    public class ChatMemberStatusHandler : UpdateHandler
    {
        private readonly IBot _bot;
        public ChatMemberStatusHandler(IBot bot)
        {
            _bot = bot;
        }
        public override async Task HandleAsync(ITelegramBotClient client, Update update)
        {
            if (update.Type == UpdateType.ChatMember &&
               update.ChatMember.NewChatMember.Status == ChatMemberStatus.Member
               && update.ChatMember.OldChatMember.Status != ChatMemberStatus.Kicked)
            {
                var user = update.ChatMember.NewChatMember.User;
                await _bot.HandleChatMemberStatusAsync(user, update.ChatMember.Chat.Id);
                await client.SendTextMessageAsync(update.ChatMember.Chat.Id, $"{user.CreateMention()} теперь может писать сообщения", ParseMode.Markdown);
            }
            await base.HandleAsync(client, update);
        }
    }
}
