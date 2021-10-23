using System;

namespace SR.Data.Enums
{
    public enum LanguageType : byte
    {
        English = 1,
        Russian = 2
    }

    public static class LanguageHelper
    {
        public static string Localize(this LanguageType language) => language switch
        {
            LanguageType.English => "🇺🇸 English",
            LanguageType.Russian => "🇷🇺 Русский",
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };
    }
}
