using KiwiBot.Data.Entities;
using KiwiBot.Data.Enumerations;
using KiwiBot.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KiwiBot.Services.Implementations
{
    class ChatService: IChatService
    {
        private readonly IGlobalRepository _repository;
        private readonly IBooruService _booruService;

        public ChatService(IGlobalRepository repository, IBooruService booruService)
        {
            _repository = repository;
            _booruService = booruService;
        }

        public async Task<Chat> FindChatAsync(long chatId)
        {
            return await _repository.FindAsync<Chat>(chatId);
        }

        public async Task<Booru> GetSelectedBooruAsync(long chatId)
        {
            return await _repository.GetSelectedBooruAsync(chatId) ?? throw new Exception("booru not found");
        }

        public async Task RegisterChatAsync(long chatId)
        {
            try
            {
                Booru defaultBooru = await _booruService.GetDefaultBooruAsync();

                Chat chat = new Chat()
                {
                   ChatId = chatId,
                   Booru = defaultBooru,
                   ChatMode = ChatModeEnum.SFW,
                };

                await _repository.AddAsync(chat);
            }
            catch(DbUpdateException)
            {
                throw new Exception("chat is already registered");
            }
        }

        public async Task UpdateChatModeAsync(Chat chat, string mode)
        {
            chat.ChatMode = (ChatModeEnum) Enum.Parse(typeof(ChatModeEnum), mode);

            await _repository.UpdateAsync(chat);
        }

        public async Task UpdateChoosenBooruAsync(Chat chat, string booru)
        {
            Booru foundBooru = await _booruService.FindBooruByNameAsync(booru) ?? throw new Exception("booru not found");
            
            chat.Booru = foundBooru;
            await _repository.UpdateAsync(chat);
        }
    }
}
