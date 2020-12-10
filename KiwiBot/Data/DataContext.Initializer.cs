using KiwiBot.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace KiwiBot.Data
{
    partial class DataContext
    {
        public void Initialize()
        {
            if (!Engines.Any())
            {
                List<Engine> engines = new List<Engine>
                {
                    new Engine
                    {
                        EngineName = "Danbooru",
                        FileUrlKey = "large_file_url",
                        TagsKey = "tag_string",
                    },
                    new Engine
                    {
                        EngineName = "Moebooru",
                        FileUrlKey = "sample_url",
                        TagsKey = "tags",
                    },
                    new Engine
                    {
                        EngineName = "Gelbooru",
                    }
                };

                Engines.AddRange(engines);
                SaveChanges();
            }

            if (!Boorus.Any())
            {
                List<Engine> engines = Engines.ToList();

                List<Booru> boorus = new List<Booru>
                {
                    new Booru
                    {
                        BooruName = "Danbooru",
                        ApiEndpoint = "https://danbooru.donmai.us/posts.json",
                        Engine = engines.FirstOrDefault(x => x.EngineName == "Danbooru"),
                    },
                    new Booru
                    {
                        BooruName = "Yandere",
                        ApiEndpoint = "https://yande.re/post.json",
                        Engine = engines.FirstOrDefault(x => x.EngineName == "Moebooru"),
                    },
                    new Booru
                    {
                        BooruName = "Konachan",
                        ApiEndpoint = "https://konachan.com/post.json",
                        Engine = engines.FirstOrDefault(x => x.EngineName == "Moebooru"),
                    },
                    new Booru
                    {
                        BooruName = "LoliBooru",
                        ApiEndpoint = "https://lolibooru.moe/post.json",
                        Engine = engines.FirstOrDefault(x => x.EngineName == "Moebooru"),
                    },
                    new Booru
                    {
                        BooruName = "SafeBooru",
                        ApiEndpoint = "https://safebooru.org/index.php",
                        Engine = engines.FirstOrDefault(x => x.EngineName == "Gelbooru"),
                        LockedMode = true,
                    },
                    new Booru
                    {
                        BooruName = "Gelbooru",
                        ApiEndpoint = "https://gelbooru.com/index.php",
                        Engine = engines.FirstOrDefault(x => x.EngineName == "Gelbooru"),
                        LockedMode = true,
                    }
                };

                Boorus.AddRange(boorus);
                SaveChanges();
            }
        }
    }
}
