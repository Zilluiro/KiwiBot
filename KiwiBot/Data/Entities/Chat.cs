using KiwiBot.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace KiwiBot.Data.Entities
{
    class Chat
    {
        [Key]
        public long ChatId { get; set; }

        [Required]
        public int BooruId { get; set; }

        [Required]
        public ChatModeEnum ChatMode { get; set; }

        public Booru Booru { get; set; }
    }
}
