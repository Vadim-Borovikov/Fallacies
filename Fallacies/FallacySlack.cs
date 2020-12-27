using System;

namespace Fallacies
{
    public class FallacySlack
    {
        public readonly string Code;
        public readonly string IconLwPath;

        public readonly FallacyFallacyMania FallacyFallacyMania;

        public FallacySlack(FallacyFallacyMania fallacyFallacyMania)
        {
            FallacyFallacyMania = fallacyFallacyMania;
            string name = GetName(FallacyFallacyMania.NameEn);
            Code = CondertToCode(name);
            IconLwPath = CondertToPath(name);
        }

        private static string GetName(string nameEn)
        {
            string name = RemovePrefix(nameEn.ToLower(), "the ");
            name = name.Replace("argumentum ", "");
            name = name.Replace("appeal to ", "");
            name = name.Replace("argument from ", "");
            name = name.Replace("argument by ", "");
            name = name.Replace(" this,", "");
            name = name.Replace(" of this", "");
            name = name.Replace("/", " ");
            name = name.Replace("'", "");
            name = name.Replace("-", " ");
            name = RemoveSuffix(name, " fallacy");
            return $"fallacy {name}";
        }

        private static string CondertToCode(string name)
        {
            string code = $":{name}:";
            code = code.Replace(' ', '_');
            return code;
        }

        private static string CondertToPath(string name)
        {
            string path = $"/sites/default/files/images/slack-emoji-{name}.png";
            path = path.Replace(' ', '-');
            return path;
        }


        private static string RemovePrefix(string s, string prefix)
        {
            return s.StartsWith(prefix, StringComparison.Ordinal) ? s.Substring(prefix.Length, s.Length - prefix.Length) : s;
        }

        private static string RemoveSuffix(string s, string suffix)
        {
            return s.EndsWith(suffix, StringComparison.Ordinal) ? s.Substring(0, s.Length - suffix.Length) : s;
        }
    }
}
