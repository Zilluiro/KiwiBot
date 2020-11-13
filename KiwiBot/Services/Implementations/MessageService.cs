using KiwiBot.BooruClients;
using KiwiBot.BooruClients.Factories;
using KiwiBot.Data.Entities;
using KiwiBot.Data.Enumerations;
using KiwiBot.Data.Repository;
using KiwiBot.DataModels;
using KiwiBot.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiwiBot.Services.Implementations
{
    class MessageService: IMessageService
    {
        private readonly IGlobalRepository _repository;
        private readonly IEnumerable<IAbstractBooruFactory> _booruFactories;
        public MessageService(IEnumerable<IAbstractBooruFactory> booruFactories, IGlobalRepository repository)
        {
            _booruFactories = booruFactories;
            _repository = repository;
        }

        private string BeautifyTags(string tags)
        {
            string pattern = @"(\+|-|\(|\)|'|\.|&|/)";
            return tags?.Split(' ').Aggregate(string.Empty, (left, right) => $"{left} #{Regex.Replace(right, pattern, "_")}");
        }

        public async Task<AbstractPostModel> GetLastPictureAsync(long chatId)
        {
            Chat chat = await FindChatWithIncludesAsync(chatId) ?? throw new Exception("chat not found");
            AbstractBooruClient booruService = GetBooruClient(chat.Booru);

            AbstractPostModel model = await booruService.GetLastPictureAsync(chat.ChatMode);
            model.Tags = BeautifyTags(model.Tags);
            return model;
        }

        public async Task<AbstractPostModel> GetRandomPictureAsync(long chatId)
        {
            Chat chat = await FindChatWithIncludesAsync(chatId) ?? throw new Exception("chat not found");
            AbstractBooruClient booruService = GetBooruClient(chat.Booru);

            AbstractPostModel model = await booruService.GetRandomPictureAsync(chat.ChatMode);
            model.Tags = BeautifyTags(model.Tags);
            return model;
        }

        public async Task UpdateChatModeAsync(long chatId, string mode)
        {
            Chat chat = await FindChatAsync(chatId);
            chat.ChatMode = (ChatModeEnum) Enum.Parse(typeof(ChatModeEnum), mode);

            await _repository.UpdateAsync(chat);
        }

        public async Task UpdateChoosenBooruAsync(long chatId, string booru)
        {
            Chat chat = await FindChatAsync(chatId);
            Booru foundBooru = await FindBooruByNameAsync(booru) ?? throw new Exception("booru not found");
            
            chat.Booru = foundBooru;
            await _repository.UpdateAsync(chat);
        }

        public async Task RegisterChatAsync(long chatId)
        {
            try
            {
                await _repository.RegisterChatAsync(chatId);
            }
            catch(DbUpdateException)
            {
                throw new Exception("chat is already registered");
            }
        }

        public async Task<Chat> FindChatAsync(long chatId)
        {
            try
            {
                return await _repository.FindAsync<Chat>((object) chatId) ?? throw new NotRegisteredException();
            }
            catch(NotRegisteredException)
            {
                return await _repository.RegisterChatAsync(chatId);
            }
        }

        private async Task<Booru> FindBooruByNameAsync(string booru)
        {
            return await _repository.FindAsync<Booru>(x => x.BooruName == booru);
        }

        public async Task<Chat> FindChatWithIncludesAsync(long chatId)
        {
            try
            {
                return await _repository.FindAsync<Chat>(x => x.ChatId == chatId, x => x.Booru) ?? throw new NotRegisteredException();
            }
            catch(NotRegisteredException)
            {
                return await _repository.RegisterChatAsync(chatId);
            }
        }

        private AbstractBooruClient GetBooruClient(Booru booru)
        {
            string factoryName = booru.ApiCompatible == true ? "Compatible" : booru.BooruName;
            IAbstractBooruFactory booruFactory = _booruFactories.Single(x => x.FactoryName == factoryName);

            return booruFactory.CreateBooruClient(booru);
        }

        public async Task<List<Booru>> GetBoorusAsync()
        {
            return await _repository.GetAllAsync<Booru>();
        }
    }
}
