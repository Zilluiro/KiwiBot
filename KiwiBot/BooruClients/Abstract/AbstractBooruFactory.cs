using KiwiBot.Data.Entities;

namespace KiwiBot.BooruClients.Abstract
{
    interface IAbstractBooruFactory
    {
        AbstractBooruClient CreateBooruClient(Booru booru);

        string[] SupportedEngines { get; }
    }
}
