using Discord;

namespace SR.Services.DiscordServices.DiscordEmbedService
{
    public interface IDiscordEmbedService
    {
        Embed BuildReactionEmbed(string color, string message, string imageUrl);
    }
}
