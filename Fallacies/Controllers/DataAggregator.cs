using System.Collections.Generic;
using System.Linq;
using Fallacies.Data;

namespace Fallacies.Controllers
{
    internal static class DataAggregator
    {
        internal static List<Fallacy> Aggregate(IEnumerable<LocalFallacy> localFallacies,
                                                IEnumerable<SlackFallacy> slackFallacies,
                                                IEnumerable<GitFallacy> gitFallacies)
        {
            List<Fallacy> fallacies = localFallacies.Select(CreateFrom).ToList();

            fallacies.AddFromSlack(slackFallacies);

            fallacies.AddFromGit(gitFallacies);

            return fallacies;
        }

        private static void AddFromSlack(this ICollection<Fallacy> fallacies, IEnumerable<SlackFallacy> slackFallacies)
        {
            foreach (SlackFallacy slackFallacy in slackFallacies)
            {
                if (slackFallacy.Code == ":fallacy_all:")
                {
                    continue;
                }

                Fallacy fallacy = fallacies.FirstOrDefault(f => NameToCode(f.Name) == slackFallacy.Code);
                if (fallacy != null)
                {
                    fallacy.AddInfo(slackFallacy);
                }
                else
                {
                    fallacies.Add(CreateFrom(slackFallacy));
                }
            }
        }

        private static void AddFromGit(this ICollection<Fallacy> fallacies, IEnumerable<GitFallacy> gitFallacies)
        {
            foreach (GitFallacy gitFallacy in gitFallacies)
            {
                if (gitFallacy.Id == 0)
                {
                    continue;
                }

                Fallacy fallacy = fallacies.FirstOrDefault(f => AreSame(f, gitFallacy));
                if (fallacy != null)
                {
                    fallacy.AddInfo(gitFallacy);
                }
                else
                {
                    fallacies.Add(CreateFrom(gitFallacy));
                }
            }
        }

        private static Fallacy CreateFrom(LocalFallacy localFallacy)
        {
            var fallacy = new Fallacy();
            fallacy.AddInfo(localFallacy);
            return fallacy;
        }

        private static Fallacy CreateFrom(SlackFallacy slackFallacy)
        {
            var fallacy = new Fallacy();
            fallacy.AddInfo(slackFallacy);
            return fallacy;
        }

        private static Fallacy CreateFrom(GitFallacy gitFallacy)
        {
            var fallacy = new Fallacy();
            fallacy.AddInfo(gitFallacy);
            return fallacy;
        }

        /*private static CustomFallacy CreateFromGit(GitFallacy gitFallacy)
        {
            var fallacy = new CustomFallacy
            {
                Id = gitFallacy.Id,
                NameEn = gitFallacy.NameEn,
                NameRu = gitFallacy.NameRu,
                GitImagePathBackground = gitFallacy.ImagePathBackground,
                GitImagePathIcon = gitFallacy.ImagePathIcon,
                Description = gitFallacy.Description,
                Examples = gitFallacy.Examples
            };

            fallacy.Name = ConvertName(fallacy.NameEn);
            fallacy.Code = GetCodeFromName(fallacy.Name);
            switch (fallacy.Code)
            {
                case ":fallacy_ad_hominem:":
                    fallacy.Code = ":fallacy_adhominem:";
                    break;
                case ":fallacy_anecdotal_evidence:":
                    fallacy.Code = ":fallacy_anecdotal:";
                    break;
                case ":fallacy_circular_logic:":
                    fallacy.Code = ":fallacy_begging_question:";
                    break;
                case ":fallacy_ad_cellarium:":
                    fallacy.Code = ":fallacy_personal_incredulity:";
                    break;
                case ":fallacy_moving_the_goalposts:":
                    fallacy.Code = ":fallacy_special_pleading:";
                    break;
            }

            return fallacy;
        }*/

        private static void AddInfo(this Fallacy fallacy, LocalFallacy localFallacy)
        {
            fallacy.Name = localFallacy.Name.StartWithCapital();
            fallacy.NameRu = localFallacy.NameRu.StartWithCapital();
            fallacy.Description = localFallacy.Synopsys;
            fallacy.Examples = localFallacy.Examples.RemovePrefix("Example: ");
        }

        private static void AddInfo(this Fallacy fallacy, SlackFallacy slackFallacy)
        {
            fallacy.Code = slackFallacy.Code;
            fallacy.SlackImage = slackFallacy.Image;

            if (string.IsNullOrWhiteSpace(fallacy.Name))
            {
                fallacy.Name = CodeToName(fallacy.Code);
            }
        }

        private static void AddInfo(this Fallacy fallacy, GitFallacy gitFallacy)
        {
            fallacy.GitId = gitFallacy.Id;
            fallacy.NameRu = gitFallacy.NameRu;
            fallacy.GitImagePathBackground = gitFallacy.ImagePathBackground;
            fallacy.GitImagePathIcon = gitFallacy.ImagePathIcon;
            fallacy.Description = gitFallacy.Description;
            fallacy.Examples = gitFallacy.Examples;

            if (string.IsNullOrWhiteSpace(fallacy.Name))
            {
                fallacy.Name = gitFallacy.NameEn;
            }
        }

        private static bool AreSame(Fallacy fallacy, GitFallacy gitFallacy)
        {
            if ((fallacy.Name == "Anecdotal") && (gitFallacy.NameEn == "Anecdotal evidence"))
            {
                return true;
            }

            if ((fallacy.Name == "Begging the question") && (gitFallacy.NameEn == "Circular logic"))
            {
                return true;
            }

            if ((fallacy.Name == "Genetic") && (gitFallacy.NameEn == "Genetic fallacy"))
            {
                return true;
            }

            if ((fallacy.Name == "Personal incredulity") && (gitFallacy.NameEn == "Appeal to incredulity"))
            {
                return true;
            }

            if ((fallacy.Name == "Special pleading") && (gitFallacy.NameEn == "Moving the goalposts"))
            {
                return true;
            }

            return fallacy.Name == gitFallacy.NameEn;
        }

        /*private static string ConvertName(string nameEn)
        {
            string name = nameEn.ToLower().RemovePrefix("the ");
            name = name.Replace("argumentum ", "");
            name = name.Replace("appeal to ", "");
            name = name.Replace("argument from ", "");
            name = name.Replace("argument by ", "");
            name = name.Replace(" this,", "");
            name = name.Replace(" of this", "");
            name = name.Replace("/", " ");
            name = name.Replace("'", "");
            name = name.Replace("-", " ");
            name = name.RemoveSuffix(" fallacy");
            return name.First().ToString().ToUpper() + name.Substring(1);
        }*/

        private static string CodeToName(string code)
        {
            string name = code.Replace(":", "");
            name = name.Replace('_', ' ');
            name = name.RemovePrefix("fallacy ");
            return name.StartWithCapital();
        }

        private static string NameToCode(string name)
        {
            string code = name.ToLower();
            code = code.Replace("appeal to ", "");
            code = code.Replace("the ", "");
            code = code.Replace("-", " ");
            code = code.Replace("/", " ");
            code = code.RemoveSuffix(" fallacy");
            code = code.RemoveSuffix("'s");
            /*code = code.Replace("argumentum ", "");
            code = code.Replace("argument from ", "");
            code = code.Replace("argument by ", "");
            code = code.Replace(" this,", "");
            code = code.Replace(" of this", "");
            code = code.Replace("'", "");*/
            return $":fallacy_{code.Replace(' ', '_')}:";
        }
    }
}
