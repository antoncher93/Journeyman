using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Journeyman.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Journeyman.App.Data;
using Journeyman.App.Handlers;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace Journeyman.App
{
    class Bot : IBot
    {
        public const string ACCEPT_AGREEMENT_CALLBACK = "ACCEPT_AGREEMENT_CALLBACK";
        private readonly ITelegramBotClient _client;
        private readonly UpdateHandler _botCommandHandler;
        private readonly BotDbContext _db;
        private readonly ILogger _logger;

        public Bot(IServiceProvider serviceProvider)
        {
            _client = serviceProvider.GetService<ITelegramBotClient>();
            _db = serviceProvider.GetService<BotDbContext>();
            _logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Bot>();
            _botCommandHandler = UpdateHandler.Create(this, serviceProvider);
        }

        public void Start(string webhook)
        {
            try
            {
                _client.SetWebhookAsync(webhook,
                    allowedUpdates: new[] {
                        UpdateType.Message,
                        UpdateType.ChatMember,
                        UpdateType.InlineQuery,
                        UpdateType.CallbackQuery
                    });
                _logger.LogInformation($"Bot started at webhook {webhook}");
            }
            catch (Exception exc)
            {
                _logger.LogError($"Error of SetWebhookAsync. {exc.Message}");
            }
        }

        public async Task HandleAsync(Update update)
        {
            var trace = JsonConvert.SerializeObject(update);
            _logger.LogInformation(trace);
            try
            {
                await _botCommandHandler.HandleAsync(_client, update);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.StackTrace);
            }
        }

        public async Task RestrictUserInChatAsync(User user, long chatId, int? replyMsgId = null)
        {
            if(string.Equals(user.Username, "anton_cher93"))
            {
                await _client.SendTextMessageAsync(chatId,
                $"Ошибка выполнения команды!",
                replyToMessageId: replyMsgId,
                parseMode: ParseMode.Markdown);
                return;
            }

            await _client.SendTextMessageAsync(chatId, 
                $"{user.CreateMention()} бан на 72 часа!", 
                replyToMessageId: replyMsgId, 
                parseMode: ParseMode.Markdown);

            var person = await _db.Persons.FirstOrDefaultAsync(p => long.Equals(p.UserId, user.Id) && long.Equals(p.ChatId, chatId));
            if(person is null)
            {
                person = new Person()
                {
                    UserId = user.Id,
                    ChatId = chatId,
                    IsBanned = true
                };
                _db.Persons.Add(person);
            }
            person.IsBanned = true;
            await _db.SaveChangesAsync();
            var untilDate = DateTime.Now + TimeSpan.FromHours(72);
            await _RestrictUserInChatAsync(user, chatId, untilDate);
        }

        public async Task DerestrictUserInChatAsync(User user, long chatId, int? replyMsgId = null)
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

            await _client.RestrictChatMemberAsync(chatId, user.Id, permissions);
        }

        private async Task _RestrictUserInChatAsync(User user, long chatId, DateTime? untilDateTime)
        {
            var permissions = new ChatPermissions()
            {
                CanSendMediaMessages = false,
                CanSendMessages = false,
                CanSendOtherMessages = false
            };
            await _client.RestrictChatMemberAsync(chatId, user.Id, permissions, untilDateTime);
        }

        public async Task AddChatAgreementAsync(Chat chat, string text)
        {
            var agreement = await _db.ChatAgreements.FirstOrDefaultAsync(a => a.ChatId == chat.Id);
            if (agreement is null)
            {
                agreement = new ChatAgreement()
                {
                    ChatId = chat.Id,
                    Text = text
                };

                _db.ChatAgreements.Add(agreement);
            }
            else
            {
                agreement.Text = text;
                _db.ChatAgreements.Update(agreement);
            }

            await _db.SaveChangesAsync();
        }

        public async Task WarnChatUserAsync(User user, long chatId, int? msgId)
        {
            var person = await _db.Persons.FirstOrDefaultAsync(p => Equals(p.UserId, user.Id) && Equals(p.ChatId, chatId));

            if (person is null)
            {
                person = new Person()
                {
                    UserId = user.Id,
                    ChatId = chatId
                };

                _db.Persons.Add(person);
                _db.SaveChanges();
            }

            var warnCount = person.Warnings;
            warnCount++;

            if (warnCount > 3)
            {
                warnCount = 0;
                await RestrictUserInChatAsync(user, chatId, msgId);
            }
            else
            {
                await _client.SendTextMessageAsync(chatId, 
                    $"{user.CreateMention()} предупреждение №{warnCount}", 
                    replyToMessageId: msgId, 
                    parseMode: ParseMode.Markdown);
            }

            person.Warnings = warnCount;
            _db.Entry(person).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task WelcomeChatMembers(long chatId, User[] users)
        {
            string text = "";
            text += users.Length > 1 ? "Уважаемые " : "Уважаемый(ая) ";
            foreach (var user in users)
            {
                text += user.CreateMention() + ", ";
                await _RestrictUserInChatAsync(user, chatId, null);
            }

            var button = new InlineKeyboardButton("Принимаю");
            button.CallbackData = ACCEPT_AGREEMENT_CALLBACK;
            var keyboard = new InlineKeyboardMarkup(button);
            var agreement = await _db.ChatAgreements.FirstOrDefaultAsync(a => a.ChatId.Equals(chatId));
            text += "\n";
            text += string.IsNullOrEmpty(agreement?.Text) 
                ? "Я не в курсе о правилах чата, так что просто нажми кнопку."
                : agreement.Text;

            await _client.SendTextMessageAsync(chatId, text,
                parseMode: ParseMode.Markdown,
                replyMarkup: keyboard);
        }

        public async Task HandleChatMemberStatusAsync(User user, long chatId)
        {
            var person = await _db.Persons.FirstOrDefaultAsync(p => long.Equals(p.UserId, user.Id) && long.Equals(p.ChatId, chatId));
            if (person is null)
            {
                person = new Person()
                {
                    UserId = user.Id,
                    ChatId = chatId,
                    IsBanned = false
                };
                _db.Persons.Add(person);
                _db.Entry(person).State = EntityState.Added;
            }
            else
            {
                person.IsBanned = false;
                _db.Entry(person).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
        }
    }
}
