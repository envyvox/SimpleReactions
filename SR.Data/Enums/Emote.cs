using System;

namespace SR.Data.Enums
{
    public enum Emote : byte
    {
        Blank = 0,
        List = 1,
        DiscordLogo = 2,
        TwitterLogo = 3,
        PineappleThinking = 4,
        PineappleReading = 5,
        PineappleLove = 6,
        Mastercard = 7
    }

    public static class EmoteHelper
    {
        public static string Display(this Emote emote) => emote switch
        {
            Emote.Blank => "<:Blank:773616910867628032>",
            Emote.List => "<:List:773616884086996993>",
            Emote.DiscordLogo => "<:DiscordLogo:773630216282046464>",
            Emote.TwitterLogo => "<:TwitterLogo:773630216433172500>",
            Emote.PineappleThinking => "<:PineappleThinking:773692108933824543>",
            Emote.PineappleReading => "<:PineappleReading:773693395050692639>",
            Emote.PineappleLove => "<:PineappleLove:773693374922227762>",
            Emote.Mastercard => "<:Mastercard:773706166881878016>",
            _ => throw new ArgumentOutOfRangeException(nameof(emote), emote, null)
        };
    }
}
