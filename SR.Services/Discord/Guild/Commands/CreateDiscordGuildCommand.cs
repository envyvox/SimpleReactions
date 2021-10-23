using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SR.Data;
using SR.Data.Entities;
using SR.Data.Enums;
using SR.Data.Extensions;
using SR.Services.Discord.Guild.Models;

namespace SR.Services.Discord.Guild.Commands
{
    public record CreateDiscordGuildCommand(long GuildId) : IRequest<DiscordGuildDto>;

    public class CreateDiscordGuildHandler : IRequestHandler<CreateDiscordGuildCommand, DiscordGuildDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public CreateDiscordGuildHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<DiscordGuildDto> Handle(CreateDiscordGuildCommand request, CancellationToken ct)
        {
            var exist = await _db.DiscordGuilds
                .AnyAsync(x => x.Id == request.GuildId);

            if (exist)
            {
                throw new Exception($"discord guild with id {request.GuildId} already exist");
            }

            var created = await _db.CreateEntity(new DiscordGuild
            {
                Id = request.GuildId,
                LanguageType = LanguageType.English,
                EmbedColor = "36393F"
            });

            return _mapper.Map<DiscordGuildDto>(created);
        }
    }
}
