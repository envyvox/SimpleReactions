using System.Globalization;
using Discord;
using SR.Framework.Autofac;

namespace SR.Services.DiscordServices.DiscordEmbedService.Impl
{
    [InjectableService]
    public class DiscordEmbedService : IDiscordEmbedService
    {
        public Embed BuildReactionEmbed(string color, string message, string imageUrl)
        {
            var embed = new EmbedBuilder()
                .WithDescription(message)
                .WithImageUrl(imageUrl)
                .WithColor(new Color(uint.Parse(color, NumberStyles.HexNumber)));

            return embed.Build();
        }
    }
}
