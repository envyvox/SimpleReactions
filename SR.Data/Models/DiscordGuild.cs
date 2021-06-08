using SR.Data.Enums;

namespace SR.Data.Models
{
    public class DiscordGuild : EntityBase
    {
        public string Prefix { get; set; }
        public Language Language { get; set; }
        public string Color { get; set; }
    }
}
