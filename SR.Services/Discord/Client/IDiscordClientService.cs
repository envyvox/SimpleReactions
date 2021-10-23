using System.Threading.Tasks;
using Discord.WebSocket;

namespace SR.Services.Discord.Client
{
    public interface IDiscordClientService
    {
        Task Start();
        Task<DiscordSocketClient> GetSocketClient();
    }
}
