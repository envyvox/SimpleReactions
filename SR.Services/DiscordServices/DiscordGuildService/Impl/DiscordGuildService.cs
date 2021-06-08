using System.Threading.Tasks;
using Dapper;
using SR.Data.Enums;
using SR.Framework.Autofac;
using SR.Framework.Database;

namespace SR.Services.DiscordServices.DiscordGuildService.Impl
{
    [InjectableService]
    public class DiscordGuildService : IDiscordGuildService
    {
        private readonly IConnectionManager _con;

        public DiscordGuildService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<string> GetGuildPrefix(long guildId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<string>(@"
                    select prefix from discord_guilds
                    where id = @guildId",
                    new {guildId});

        public async Task<Language> GetGuildLanguage(long guildId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<Language>(@"
                    select language from discord_guilds
                    where id = @guildId",
                    new {guildId});

        public async Task<string> GetGuildColor(long guildId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<string>(@"
                    select color from discord_guilds
                    where id = @guildId",
                    new {guildId});

        public async Task AddDiscordGuildToDb(long guildId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into discord_guilds(id)
                    values (@guildId)",
                    new {guildId});

        public async Task DeleteDiscordGuildFromDb(long guildId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from discord_guilds
                    where id = @guildId",
                    new {guildId});

        public async Task UpdateGuildPrefix(long guildId, string newPrefix) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update discord_guilds
                    set prefix = @newPrefix,
                        updated_at = now()
                    where id = @guildId",
                    new {guildId, newPrefix});

        public async Task UpdateGuildLanguage(long guildId, Language newLanguage) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update discord_guilds
                    set language = @newLanguage,
                        updated_at = now()
                    where id = @guildId",
                    new {guildId, newLanguage});

        public async Task UpdateGuildColor(long guildId, string newColor) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update discord_guilds
                    set color = @newColor,
                        updated_at = now()
                    where id = @guildId",
                    new {guildId, newColor});
    }
}
