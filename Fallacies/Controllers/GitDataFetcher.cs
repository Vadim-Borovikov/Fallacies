using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Fallacies.Data;

namespace Fallacies.Controllers
{
    internal class GitDataFetcher
    {
        internal static List<GitFallacy> FetchData()
        {
            using (var client = new WebClient())
            {
                string[] descriptionRu = FetchTextFile(client, GitConfig.DescriptionsRuUrl);
                string[] descriptionEn = FetchTextFile(client, GitConfig.DescriptionsEnUrl);

                string[] files = GetBackgroungFiles(client).ToArray();

                return LoadFallacies(descriptionRu, descriptionEn, files).ToList();
            }
        }

        private static string[] FetchTextFile(WebClient client, string url)
        {
            using (Stream stream = client.OpenRead(url))
            {
                if (stream == null)
                {
                    return new string[0];
                }
                using (var reader = new StreamReader(stream))
                {
                    string source = reader.ReadToEnd();
                    source = source.Remove(source.Length - 1);
                    string converted = Convert(source);
                    return converted.Split('\n');
                }
            }
        }

        private static string Convert(string source)
        {
            string result = source.Replace("\\\\n", " ");
            result = result.Replace("\n\"", "\n«");
            result = result.Replace(" \"", " «");
            result = result.Replace("\"", "»");
            result = result.Replace(".»", "».");
            result = result.Replace("- ", "— ");
            return result;
        }

        private static IEnumerable<string> GetBackgroungFiles(WebClient client)
        {
            string html = client.DownloadString(GitConfig.BackgroungFilesPageUrl);
            foreach (string match in GetMatches(html, GitConfig.BackgroundFileNamePattern))
            {
                yield return $"{GitConfig.BackgroungFilesUrl}/{match.Replace("\"", "")}";
            }
        }

        private static IEnumerable<string> GetMatches(string input, string pattern)
        {
            return Regex.Matches(input, pattern).Cast<Match>().Select(match => match.Value);
        }

        private static IEnumerable<GitFallacy> LoadFallacies(IReadOnlyList<string> descriptionRu,
                                                          IReadOnlyList<string> descriptionEn,
                                                          IReadOnlyList<string> files)
        {
            int fallacies = descriptionRu.Count / 4;
            for (int i = 0; i < fallacies; ++i)
            {
                yield return new GitFallacy
                {
                    Id = i,
                    NameRu = descriptionRu[i*4],
                    NameEn = descriptionEn[i*4],
                    ImagePathBackground = files[i],
                    ImagePathIcon = $"{GitConfig.UrlPrefixRawContent}{descriptionRu[i*4 + 1]}",
                    Description = descriptionRu[i*4 + 2],
                    Examples = descriptionRu[i*4 + 3]
                };
            }
        }
    }
}
