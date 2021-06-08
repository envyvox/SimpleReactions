using System;

namespace SR.Data.Enums
{
    public enum Language : byte
    {
        English = 1,
        Russian = 2
    }

    public static class LanguageHelper
    {
        public static string Localize(this Language language) => language switch
        {
            Language.English => "🇺🇸 English",
            Language.Russian => "🇷🇺 Русский",
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };
    }
}
