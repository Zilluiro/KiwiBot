using KiwiBot.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace KiwiBot.Data
{
    partial class DataContext
    {
        public void Initialize()
        {
            if (!Boorus.Any())
            {
                List<Booru> boorus = new List<Booru>
                {
                    new Booru
                    {
                        BooruName = "Danbooru",
                        ApiEndpoint = "https://danbooru.donmai.us/posts.json",
                        ApiCompatible = true,
                        FileUrlKey = "large_file_url",
                        TagsKey = "tag_string",
                    },
                    new Booru
                    {
                        BooruName = "Yandere",
                        ApiEndpoint = "https://yande.re/post.json",
                        ApiCompatible = true,
                        FileUrlKey = "sample_url",
                        TagsKey = "tags",
                    }
                };

                Boorus.AddRange(boorus);
                SaveChanges();
            }
        }
    }
}
