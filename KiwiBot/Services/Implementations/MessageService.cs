using KiwiBot.BooruClients.Abstract;
using KiwiBot.Data.Entities;
using KiwiBot.Data.Repository;
using KiwiBot.DataModels;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiwiBot.Services.Implementations
{
    class MessageService: IMessageService
    {
        private readonly IGlobalRepository _repository;
        private readonly IChatService _chatService;
        private readonly IBooruService _booruService;

        public MessageService(IGlobalRepository repository, IChatService chatService, IBooruService booruService)
        {
            _repository = repository;
            _chatService = chatService;
            _booruService = booruService;
        }

        private string BeautifyTags(string tags)
        {
            string pattern = @"(\+|-|\(|\)|'|\.|&|/|\?|~)";
            return tags?.Split(' ').Aggregate(string.Empty, 
                (left, right) => $"{left}" +
            $" {(right.Length > 1 == true ? "#": string.Empty)}{Regex.Replace(right, pattern, "_")}");
        }

        public async Task<BasePostModel> GetLastPictureAsync(Chat chat)
        {
            Booru booru = await _chatService.GetSelectedBooruAsync(chat.ChatId);
            AbstractBooruClient booruService = _booruService.GetBooruClient(booru);

            BasePostModel model = await booruService.GetLastPictureAsync(chat.ChatMode, booru.LockedMode);
            model.Tags = BeautifyTags(model.Tags);
            return model;
        }

        public async Task<BasePostModel> GetRandomPictureAsync(Chat chat)
        {
            Booru booru = await _chatService.GetSelectedBooruAsync(chat.ChatId);
            AbstractBooruClient booruService = _booruService.GetBooruClient(booru);

            BasePostModel model = await booruService.GetRandomPictureAsync(chat.ChatMode, booru.LockedMode);
            model.Tags = BeautifyTags(model.Tags);
            return model;
        }
    }
}
