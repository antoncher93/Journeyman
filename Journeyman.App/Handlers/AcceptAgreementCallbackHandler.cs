using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.App.Handlers
{
    public class AcceptAgreementCallbackHandler : UpdateHandler
    {
        private readonly IBot _bot;
        public AcceptAgreementCallbackHandler(IBot bot)
        {
            _bot = bot;
        }
        public override async Task HandleAsync(ITelegramBotClient client, Update update)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery
                && update.CallbackQuery.Data.Equals(Bot.ACCEPT_AGREEMENT_CALLBACK))
            {
                var user = update.CallbackQuery.From;
                await _bot.DerestrictUserInChatAsync(user, update.CallbackQuery.Message.Chat.Id);
            }
            else await base.HandleAsync(client, update);
        }
    }
}
