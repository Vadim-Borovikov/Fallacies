using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Fallacies.Data;
using Newtonsoft.Json.Linq;

namespace Fallacies.Controllers
{
    internal class SlackDataFetcher
    {
        internal static List<SlackFallacy> FetchData()
        {
            return EnumerateFallacies().ToList();
        }

        private static IEnumerable<SlackFallacy> EnumerateFallacies()
        {
            string json = GetEmojiJson();

            Dictionary<string, string> emoji = ExtractEmoji(json);
            Dictionary<string, string> fallaciesEmoji =
                emoji.Where(p => p.Key.Contains(SlackConfig.EmojiFilter)).ToDictionary();

            Dictionary<string, string> aliases =
                fallaciesEmoji.Where(p => p.Value.StartsWith(SlackConfig.EmojiAliasPrefix, StringComparison.Ordinal)).ToDictionary();
            Dictionary<string, string> direct = fallaciesEmoji.Except(aliases).ToDictionary();

            foreach (string key in direct.Keys)
            {
                yield return new SlackFallacy
                {
                    Code = $":{key}:",
                    Image = fallaciesEmoji[key]
                };
            }

            foreach (string key in aliases.Keys)
            {
                string alias = fallaciesEmoji[key].RemovePrefix(SlackConfig.EmojiAliasPrefix);
                if (direct.Keys.Contains(alias))
                {
                    continue;
                }
                yield return new SlackFallacy
                {
                    Code = $":{key}:",
                    Image = emoji[alias]
                };
            }
        }

        private static Dictionary<string, string> ExtractEmoji(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken jToken = jObject.GetValue(SlackConfig.EmojiJsonKey);

            return jToken.ToObject<Dictionary<string, string>>();
        }

        private static string GetEmojiJson()
        {
            WebRequest request = WebRequest.Create(SlackConfig.EmojiListRequestUrl);
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
