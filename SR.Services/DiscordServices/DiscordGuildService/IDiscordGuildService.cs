using System.Threading.Tasks;
using SR.Data.Entities;
using SR.Data.Enums;

namespace SR.Services.DiscordServices.DiscordGuildService
{
    public interface IDiscordGuildService
    {
        Task<string> GetGuildPrefix(long guildId);
        Task<Language> GetGuildLanguage(long guildId);
        Task<string> GetGuildColor(long guildId);
        Task<DiscordGuild> AddDiscordGuildToDb(long guildId);
        Task DeleteDiscordGuildFromDb(long guildId);
        Task UpdateGuildPrefix(long guildId, string newPrefix);
        Task UpdateGuildLanguage(long guildId, Language newLanguage);
        Task UpdateGuildColor(long guildId, string newColor);
    }
}
