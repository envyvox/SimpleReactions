using System;
using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SR.Data;
using SR.Data.Entities;
using SR.Data.Enums;
using SR.Framework.Autofac;

namespace SR.Services.DiscordServices.DiscordGuildService.Impl
{
    [InjectableService]
    public class DiscordGuildService : IDiscordGuildService
    {
        private readonly AppDbContext _db;
        private readonly IMemoryCache _cache;

        private const string GuildPrefixKey = "guild_{0}_prefix";
        private const string GuildLanguageKey = "guild_{0}_language";
        private const string GuildColorKey = "guild_{0}_color";

        private static readonly MemoryCacheEntryOptions DefaultCacheOptions =
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        public DiscordGuildService(AppDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<string> GetGuildPrefix(long guildId)
        {
            if (_cache.TryGetValue(string.Format(GuildPrefixKey, guildId), out string prefix)) return prefix;

            var guild = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == guildId);

            if (guild is null)
            {
                guild = await AddDiscordGuildToDb(guildId);
            }

            prefix = guild.Prefix;

            _cache.Set(string.Format(GuildPrefixKey, guildId), prefix, DefaultCacheOptions);

            return prefix;
        }

        public async Task<Language> GetGuildLanguage(long guildId)
        {
            if (_cache.TryGetValue(string.Format(GuildLanguageKey, guildId), out Language language)) return language;

            var guild = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == guildId);

            if (guild is null)
            {
                guild = await AddDiscordGuildToDb(guildId);
            }

            language = guild.Language;

            _cache.Set(string.Format(GuildLanguageKey, guildId), language, DefaultCacheOptions);

            return language;
        }

        public async Task<string> GetGuildColor(long guildId)
        {
            if (_cache.TryGetValue(string.Format(GuildColorKey, guildId), out string color)) return color;

            var guild = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == guildId);

            if (guild is null)
            {
                guild = await AddDiscordGuildToDb(guildId);
            }

            color = guild.Color;

            _cache.Set(string.Format(GuildColorKey, guildId), color, DefaultCacheOptions);

            return color;
        }

        public async Task<DiscordGuild> AddDiscordGuildToDb(long guildId)
        {
            var created = await _db.DiscordGuilds.AddAsync(new DiscordGuild
            {
                Id = guildId,
                Prefix = "..",
                Language = Language.English,
                Color = "36393F"
            });

            await _db.SaveChangesAsync();

            return created.Entity;
        }

        public async Task DeleteDiscordGuildFromDb(long guildId)
        {
            var discordGuild = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == guildId);

            if (discordGuild is null)
            {
                throw new ObjectNotFoundException();
            }

            _db.DiscordGuilds.Remove(discordGuild);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateGuildPrefix(long guildId, string newPrefix)
        {
            var discordGuild = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == guildId);

            if (discordGuild is null)
            {
                throw new ObjectNotFoundException();
            }

            discordGuild.Prefix = newPrefix;

            await _db.SaveChangesAsync();

            _cache.Set(string.Format(GuildPrefixKey, guildId), newPrefix, DefaultCacheOptions);
        }

        public async Task UpdateGuildLanguage(long guildId, Language newLanguage)
        {
            var discordGuild = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == guildId);

            if (discordGuild is null)
            {
                throw new ObjectNotFoundException();
            }

            discordGuild.Language = newLanguage;

            await _db.SaveChangesAsync();

            _cache.Set(string.Format(GuildLanguageKey, guildId), newLanguage, DefaultCacheOptions);
        }

        public async Task UpdateGuildColor(long guildId, string newColor)
        {
            var discordGuild = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == guildId);

            if (discordGuild is null)
            {
                throw new ObjectNotFoundException();
            }

            discordGuild.Color = newColor;

            await _db.SaveChangesAsync();

            _cache.Set(string.Format(GuildColorKey, guildId), newColor, DefaultCacheOptions);
        }
    }
}
