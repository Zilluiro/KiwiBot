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
        public bool ApiCompatible { get; set; }

        public string TagsKey { get; set; }

        public string FileUrlKey { get; set; }

        public List<Chat> Chats { get; set; }
    }
}
