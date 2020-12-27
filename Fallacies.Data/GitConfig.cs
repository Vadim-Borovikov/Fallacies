namespace Fallacies.Data
{
    public static class GitConfig
    {
        public static readonly string DescriptionsRuUrl = $"{UrlPrefixRawContent}fallacies-rus.txt";
        public static readonly string DescriptionsEnUrl = $"{UrlPrefixRawContent}fallacies-eng.txt";

        public static readonly string BackgroungFilesPageUrl = $"{UrlPrefix}colors";
        public static readonly string BackgroungFilesUrl = $"{UrlPrefixRawContent}colors";

        public const string BackgroundFileNamePattern = @"""([a-zA-Z0-9_]+)\.png""";

        public const string UrlPrefixRawContent =
            "https://raw.githubusercontent.com/Alexander230/fallacymania/master/src/";

        private const string UrlPrefix = "https://github.com/Alexander230/fallacymania/tree/master/src/";
    }
}