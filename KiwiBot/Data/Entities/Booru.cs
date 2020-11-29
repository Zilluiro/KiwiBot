using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiwiBot.Data.Entities
{
    class Booru
    {
        [Key]
        public int BooruId { get; set; }

        [Required]
        public string BooruName { get; set; }

        [Required]
        public string ApiEndpoint { get;set; }

        [Required]
        public int EngineId { get;set; }

        public Engine Engine { get; set; }
        public List<Chat> Chats { get; set; }
    }
}
