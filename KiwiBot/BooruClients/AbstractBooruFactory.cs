using KiwiBot.Data.Entities;

namespace KiwiBot.BooruClients.Factories
{
    interface IAbstractBooruFactory
    {
        AbstractBooruClient CreateBooruClient(Booru booru);

        string FactoryName { get; }
    }
}
