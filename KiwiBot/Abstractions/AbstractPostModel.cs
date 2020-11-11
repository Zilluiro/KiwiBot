namespace KiwiBot.Abstractions
{
    abstract class AbstractPostModel
    {
        public virtual string Tags { get; set; }

        public virtual string FileUrl { get; set; }
    }
}
