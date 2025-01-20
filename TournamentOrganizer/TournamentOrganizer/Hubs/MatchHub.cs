using Microsoft.AspNetCore.SignalR;

namespace TournamentOrganizer.api.Hubs
{
    public class MatchHub : Hub
    {
        public const string MatchUpdated = "MatchUpdated";
        public const string TournamentUpdated = "TournamentUpdated";

        public async Task JoinTournament(string tournamentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tournamentId);
        }

        public async Task LeaveTournament(string tournamentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, tournamentId);
        }
    }
}
