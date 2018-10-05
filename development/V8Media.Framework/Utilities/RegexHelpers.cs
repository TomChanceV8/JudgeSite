using System.Text.RegularExpressions;

namespace V8Media.Framework.Utilities
{
    public static class RegexHelpers
    {
        public static Regex UmbracoMediaSrc = new Regex(" src=(?:\"|')(/media/.*?)(?:\"|')", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex UmbracoMediaHref = new Regex(" href=(?:\"|')(/media/.*?)(?:\"|')", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex Email = new Regex(@"([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})");
    }
}