using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.App.BotCommands
{
    public interface IBotCommand
    {
        Task ExecuteAsync(ITelegramBotClient client, Update update);
    }
}
