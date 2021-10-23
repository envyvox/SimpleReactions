using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SR.Data;
using SR.Services.Discord.Guild.Commands;
using SR.Services.Discord.Guild.Models;

namespace SR.Services.Discord.Guild.Queries
{
    public record GetDiscordGuildQuery(long GuildId) : IRequest<DiscordGuildDto>;

    public class GetDiscordGuildHandler : IRequestHandler<GetDiscordGuildQuery, DiscordGuildDto>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public GetDiscordGuildHandler(
            DbContextOptions options,
            IMapper mapper,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<DiscordGuildDto> Handle(GetDiscordGuildQuery request, CancellationToken ct)
        {
            var entity = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == request.GuildId);

            if (entity is null)
            {
                return await _mediator.Send(new CreateDiscordGuildCommand(request.GuildId));
            }

            return _mapper.Map<DiscordGuildDto>(entity);
        }
    }
}
