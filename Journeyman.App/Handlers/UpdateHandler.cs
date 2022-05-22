using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.App.Handlers
{
    public abstract class UpdateHandler
    {
        private UpdateHandler _next;

        public UpdateHandler SetNext(UpdateHandler next)
        {
            _next = next;
            return _next;
        }

        public virtual async Task HandleAsync(ITelegramBotClient client, Update update)
        {
            if(_next != null) await _next?.HandleAsync(client, update);
        }

        public static UpdateHandler Create(IBot bot, IServiceProvider serviceProvider)
        {
            var head = new BotCommandHandler(serviceProvider, bot);
            head.SetNext(new ChatMemberStatusHandler(bot))
                .SetNext(new AcceptAgreementCallbackHandler(bot))
                .SetNext(new NewChatMembersHandler(bot));
            return head;
        }
    }
}
