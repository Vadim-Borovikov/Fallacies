using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Fallacies
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var client = new WebClient())
            {
                string[] descriptionRu = FetchTextFile(client, $"{UrlPrefix}fallacies-rus.txt");
                string[] descriptionEn = FetchTextFile(client, $"{UrlPrefix}fallacies-eng.txt");

                string[] files = GetBackFiles(client, "https://github.com/Alexander230/fallacymania/tree/master/src/colors").ToArray();

                FallacyFallacyMania[] fallacyManiaFallacies = LoadFallacies(descriptionRu, descriptionEn, files).ToArray();
                string html = GenerateHtml(fallacyManiaFallacies);
                File.WriteAllText(@"C:/Code/Fallacies/Fallacies/fallacymania.html", html);

                FallacySlack[] slackFallacies = fallacyManiaFallacies.Select(f => new FallacySlack(f)).ToArray();

                html = GenerateSlackHtml(slackFallacies, false);
                File.WriteAllText(@"C:/Code/Fallacies/Fallacies/slacktemp.html", html);

                html = GenerateSlackHtml(slackFallacies, true);
                File.WriteAllText(@"C:/Code/Fallacies/Fallacies/slack.html", html);
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

        private static IEnumerable<FallacyFallacyMania> LoadFallacies(IReadOnlyList<string> descriptionRu,
                                                                      IReadOnlyList<string> descriptionEn,
                                                                      IReadOnlyList<string> files)
        {
            int fallacies = descriptionRu.Count / 4;
            for (int i = 1; i < fallacies; ++i)
            {
                yield return new FallacyFallacyMania(i, descriptionRu[i*4], descriptionEn[i*4], files[i],
                                                     $"{UrlPrefix}{descriptionRu[i*4 + 1]}", descriptionRu[i*4 + 2],
                                                     descriptionRu[i*4 + 3]);
            }
        }

        private static IEnumerable<string> GetBackFiles(WebClient client, string url)
        {
            string html = client.DownloadString(url);
            const string Pattern = @"""([a-zA-Z0-9_]+)\.png""";
            foreach (string match in GetMatches(html, Pattern))
            {
                yield return $"{UrlPrefix}colors/{match.Replace("\"", "")}";
            }
        }

        private static IEnumerable<string> GetMatches(string input, string pattern)
        {
            // return Regex.Matches(input, pattern).Cast<Match>().Select(match => match.Groups[1].Value);
            return Regex.Matches(input, pattern).Cast<Match>().Select(match => match.Value);
        }

        private static string GenerateHtml(IEnumerable<FallacyFallacyMania> fallacies)
        {
            var sb = new StringBuilder();
            sb.Append("<html>\n");
            sb.Append("<head>\n");
            sb.Append("<title>Fallacies</title>\n");
            sb.Append("</head>\n");
            sb.Append("<body>\n");
            sb.Append("<table>\n");
            sb.Append("<tr><th /><th /><th>Name</th><th>Название</th><th>Описание</th><th>Пример(ы)</th></tr>\n");

            foreach (FallacyFallacyMania fallacy in fallacies)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{fallacy.Id}</td>");
                sb.Append($"<td><p style=\"background-image: url({fallacy.IconBackPath}\">");
                sb.Append($"<img src=\"{fallacy.IconPath}\"></p></td>");
                sb.Append($"<td>{fallacy.NameEn}</td>");
                sb.Append($"<td>{fallacy.NameRu}</td>");
                sb.Append($"<td>{fallacy.Description}</td>");
                sb.Append($"<td>{fallacy.Examples}</td>");
                sb.Append("</tr>\n");
            }

            sb.Append("</table>\n");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        private static string GenerateSlackHtml(IEnumerable<FallacySlack> fallacies, bool final)
        {
            var sb = new StringBuilder();
            sb.Append("<html>\n");
            sb.Append("<head>\n");
            sb.Append("<title>Fallacies</title>\n");
            sb.Append("</head>\n");
            sb.Append("<body>\n");
            GenerateTable(sb, fallacies, final, !final, final);
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        private static void GenerateTable(StringBuilder sb, IEnumerable<FallacySlack> fallacies, bool includeIntro,
                                          bool includeIcon, bool lwPathAsImage)
        {
            if (includeIntro)
            {
                sb.Append("А ещё есть пара десятков специальных реакций с префиксом <code>fallacy_</code> про разные <a href=\"http://fallacymania.com/\">псевдоаргументы</a>:");
            }
            sb.Append("<table>\n");
            sb.Append("<tr>");
            if (includeIcon)
            {
                sb.Append("<th />");
            }
            sb.Append("<th>Emoji</th><th>Код</th><th>Название</th><th>Описание</th><th>Пример(ы)</th></tr>\n");

            foreach (FallacySlack fallacy in fallacies)
            {
                sb.Append("<tr>");
                if (includeIcon)
                {
                    sb.Append($"<td><p style=\"background-image: url({fallacy.FallacyFallacyMania.IconBackPath})\">");
                    sb.Append($"<img src=\"{fallacy.FallacyFallacyMania.IconPath}\"></p></td>");
                }
                sb.Append(lwPathAsImage
                    ? $"<td><img src=\"{fallacy.IconLwPath}\" style=\"width: 24px; position: relative; top: 6px\"></td>"
                    : $"<td>{fallacy.IconLwPath}</td>");
                sb.Append($"<td><code>{fallacy.Code}</code></td>");
                sb.Append($"<td>{fallacy.FallacyFallacyMania.NameRu}</td>");
                sb.Append($"<td>{fallacy.FallacyFallacyMania.Description}</td>");
                sb.Append($"<td>{fallacy.FallacyFallacyMania.Examples}</td>");
                sb.Append("</tr>\n");
            }

            sb.Append("</table>\n");
        }

        private const string UrlPrefix = "https://raw.githubusercontent.com/Alexander230/fallacymania/master/src/";
    }
}
