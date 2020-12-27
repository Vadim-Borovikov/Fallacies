using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fallacies.Data;

namespace Fallacies.Controllers
{
    internal static class LocalDataFetcher
    {
        internal static List<LocalFallacy> FetchData()
        {
            string[] descriptions = File.ReadAllLines(LocalConfig.DescriptionsPath);
            string[] overrides = File.ReadAllLines(LocalConfig.OverridePath);
            List<LocalFallacy> fallacies = LoadFallacies(descriptions).ToList();
            fallacies.Override(overrides);
            return fallacies;
        }

        private static IEnumerable<LocalFallacy> LoadFallacies(IReadOnlyList<string> descriptions)
        {
            int fallacies = descriptions.Count / 4;
            for (int i = 0; i < fallacies; ++i)
            {
                yield return new LocalFallacy
                {
                    Name = descriptions[i*4],
                    Synopsys = descriptions[i*4 + 1],
                    Description = descriptions[i*4 + 2],
                    Examples = descriptions[i*4 + 3]
                };
            }
        }

        private static void Override(this ICollection<LocalFallacy> fallacies, IReadOnlyList<string> overrides)
        {
            int totalOverrides = overrides.Count / 4;
            for (int i = 0; i < totalOverrides; ++i)
            {
                string name = overrides[i*4];
                LocalFallacy fallacy = fallacies.FirstOrDefault(f => f.Name == name);
                if (fallacy == null)
                {
                    fallacy = new LocalFallacy { Name = name };
                    fallacies.Add(fallacy);
                }
                fallacy.NameRu = overrides[i*4 + 1];
                fallacy.Synopsys = overrides[i*4 + 2];
                fallacy.Examples = overrides[i*4 + 3];
            }
        }
    }
}
