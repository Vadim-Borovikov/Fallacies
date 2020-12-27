namespace Model.Slack
{
    internal static class Config
    {
        internal static readonly string DescriptionsRuUrl = $"{UrlPrefixRawContent}fallacies-rus.txt";
        internal static readonly string DescriptionsEnUrl = $"{UrlPrefixRawContent}fallacies-eng.txt";

        internal static readonly string BackgroungFilesPageUrl = $"{UrlPrefix}colors";
        internal static readonly string BackgroungFilesUrl = $"{UrlPrefixRawContent}colors";

        internal const string BackgroundFileNamePattern = @"""([a-zA-Z0-9_]+)\.png""";

        internal const string UrlPrefixRawContent =
            "https://raw.githubusercontent.com/Alexander230/fallacymania/master/src/";

        private const string UrlPrefix = "https://github.com/Alexander230/fallacymania/tree/master/src/";
    }
}