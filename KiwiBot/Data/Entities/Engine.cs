using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiwiBot.Data.Entities
{
    class Engine
    {
        [Key]
        public int EngineId { get; set; }

        [Required]
        public string EngineName { get; set; }

        public string TagsKey { get; set; }

        public string FileUrlKey { get; set; }

        public List<Booru> Boorus { get; set; }
    }
}
