using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using MediatR;
using Newtonsoft.Json;
using IRequest = MediatR.IRequest;

namespace SR.Services.Discord.Client.Commands
{
    public record BulkOverwriteGuildCommandsCommand(
            DiscordSocketRestClient RestClient,
            ApplicationCommandProperties[] Commands,
            ulong GuildId)
        : IRequest;

    public class BulkOverwriteGuildCommandsHandler : IRequestHandler<BulkOverwriteGuildCommandsCommand>
    {
        public async Task<Unit> Handle(BulkOverwriteGuildCommandsCommand request, CancellationToken ct)
        {
            try
            {
                await request.RestClient.BulkOverwriteGuildCommands(request.Commands, request.GuildId);
            }
            catch (ApplicationCommandException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Error, Formatting.Indented);
                Console.WriteLine(json);
            }

            return Unit.Value;
        }
    }
}
