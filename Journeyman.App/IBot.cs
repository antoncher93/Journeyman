using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Journeyman.App
{
    public interface IBot
    {
        void Start(string webhook);
        Task RestrictUserInChatAsync(User user, long chatId, int? replyMsgId = null);
        Task DerestrictUserInChatAsync(User user, long chatId, int? replyMsgId = null);
        Task WelcomeChatMembers(long chatId, User[] users);
        Task WarnChatUserAsync(User user, long chatId, int? msgId);
        Task HandleAsync(Update update);
        Task AddChatAgreementAsync(Chat chat, string text);
        Task HandleChatMemberStatusAsync(User user, long chatId);
        Task<bool> CanExecuteCommandAsync(User from, Chat chat);
    }
}
