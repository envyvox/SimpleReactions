using System;
using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SR.Data;
using SR.Data.Enums;
using SR.Data.Models;
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

            prefix = await _db.DiscordGuilds
                .AsQueryable()
                .Where(x => x.Id == guildId)
                .Select(x => x.Prefix)
                .SingleOrDefaultAsync();

            _cache.Set(string.Format(GuildPrefixKey, guildId), prefix, DefaultCacheOptions);

            return prefix;
        }

        public async Task<Language> GetGuildLanguage(long guildId)
        {
            if (_cache.TryGetValue(string.Format(GuildLanguageKey, guildId), out Language language)) return language;

            language = await _db.DiscordGuilds
                .AsQueryable()
                .Where(x => x.Id == guildId)
                .Select(x => x.Language)
                .SingleOrDefaultAsync();

            _cache.Set(string.Format(GuildLanguageKey, guildId), language, DefaultCacheOptions);

            return language;
        }

        public async Task<string> GetGuildColor(long guildId)
        {
            if (_cache.TryGetValue(string.Format(GuildColorKey, guildId), out string color)) return color;

            color = await _db.DiscordGuilds
                .AsQueryable()
                .Where(x => x.Id == guildId)
                .Select(x => x.Color)
                .SingleOrDefaultAsync();

            _cache.Set(string.Format(GuildColorKey, guildId), color, DefaultCacheOptions);

            return color;
        }

        public async Task AddDiscordGuildToDb(long guildId)
        {
            await _db.DiscordGuilds.AddAsync(new DiscordGuild {Id = guildId});
            await _db.SaveChangesAsync();
        }

        public async Task DeleteDiscordGuildFromDb(long guildId)
        {
            var discordGuild = await _db.DiscordGuilds
                .AsQueryable()
                .Where(x => x.Id == guildId)
                .SingleOrDefaultAsync();

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
                .AsQueryable()
                .Where(x => x.Id == guildId)
                .SingleOrDefaultAsync();

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
                .AsQueryable()
                .Where(x => x.Id == guildId)
                .SingleOrDefaultAsync();

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
                .AsQueryable()
                .Where(x => x.Id == guildId)
                .SingleOrDefaultAsync();

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
