namespace KiwiBot.Helpers.Converters
{
    interface IDataConverter
    {
        public string To<T>(T obj) where T : class;

        public T From<T>(string str) where T : class;
    }
}
