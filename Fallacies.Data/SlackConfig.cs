namespace Fallacies.Data
{
    public static class SlackConfig
    {
        public static readonly string EmojiListRequestUrl = $"https://slack.com/api/emoji.list?token={Token}";
        public const string EmojiJsonKey = "emoji";
        public const string EmojiFilter = "fallacy";
        public const string EmojiAliasPrefix = "alias:";

        private const string Token = "xoxp-12931160019-89669594468-115427995200-2ad324dc459ddf8bb7a656f95d9e69fc";
    }
}