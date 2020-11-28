using KiwiBot.BooruClients;
using KiwiBot.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KiwiBot.Services
{
    interface IBooruService
    {
        Task<Booru> GetDefaultBooruAsync();
        Task<Booru> FindBooruByNameAsync(string booru);
        AbstractBooruClient GetBooruClient(Booru booru);
        Task<List<Booru>> GetBoorusAsync();
    }
}
