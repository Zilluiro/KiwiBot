using KiwiBot.BooruClients.Abstract;
using KiwiBot.Data.Entities;
using KiwiBot.Data.Repository;
using KiwiBot.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KiwiBot.Services.Implementations
{
    class BooruService: IBooruService
    {
        private readonly IGlobalRepository _repository;
        private readonly IEnumerable<IAbstractBooruFactory> _booruFactories;
        private readonly BotSettings _configuration;

        public BooruService(IGlobalRepository repository, IEnumerable<IAbstractBooruFactory> booruFactories, 
            IOptions<BotSettings> configuration)
        {
            _repository = repository;
            _configuration = configuration.Value;
            _booruFactories = booruFactories;
        }

        public async Task<Booru> GetDefaultBooruAsync()
        {
            return await _repository.FindAsync<Booru>(x => x.BooruName == _configuration.DefaultBooru)
                ?? throw new Exception("default booru not found");
        }

        public async Task<Booru> FindBooruByNameAsync(string booru)
        {
            return await _repository.FindAsync<Booru>(x => x.BooruName == booru);
        }

        public AbstractBooruClient GetBooruClient(Booru booru)
        {
            string engine = booru.Engine.EngineName;
            IAbstractBooruFactory booruFactory = _booruFactories.Single(x => x.Engine.Contains(engine));

            return booruFactory.CreateBooruClient(booru);
        }

        public async Task<List<Booru>> GetBoorusAsync()
        {
            return await _repository.GetAllAsync<Booru>();
        }
    }
}
