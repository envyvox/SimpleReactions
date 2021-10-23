using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SR.Data;
using SR.Data.Enums;
using SR.Data.Extensions;

namespace SR.Services.Reaction.Queries
{
    public record GetRandomReactionImageUrlQuery(ReactionType Type) : IRequest<string>;

    public class GetRandomReactionImageUrlHandler : IRequestHandler<GetRandomReactionImageUrlQuery, string>
    {
        private readonly AppDbContext _db;

        public GetRandomReactionImageUrlHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<string> Handle(GetRandomReactionImageUrlQuery request, CancellationToken ct)
        {
            var entity = await _db.Reactions
                .OrderByRandom()
                .Where(x => x.Type == request.Type)
                .Take(1)
                .Select(x => x.Url)
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new Exception($"there is no images for {request.Type.ToString()} reaction");
            }

            return entity;
        }
    }
}
