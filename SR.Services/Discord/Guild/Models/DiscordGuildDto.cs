using AutoMapper;
using SR.Data.Entities;
using SR.Data.Enums;

namespace SR.Services.Discord.Guild.Models
{
    public record DiscordGuildDto(
        long Id,
        LanguageType LanguageType,
        string EmbedColor);

    public class DiscordGuildProfile : Profile
    {
        public DiscordGuildProfile() => CreateMap<DiscordGuild, DiscordGuildDto>();
    }
}
