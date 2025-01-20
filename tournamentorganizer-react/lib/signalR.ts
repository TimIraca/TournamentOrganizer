import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";

class SignalRService {
  private connection: HubConnection | null = null;
  private static instance: SignalRService;

  private constructor() {}

  public static getInstance(): SignalRService {
    if (!SignalRService.instance) {
      SignalRService.instance = new SignalRService();
    }
    return SignalRService.instance;
  }

  async startConnection() {
    try {
      this.connection = new HubConnectionBuilder()
        .withUrl("/api/hubs/match")
        .withAutomaticReconnect()
        .build();

      await this.connection.start();
      console.log("SignalR Connected!");
    } catch (err) {
      console.error("SignalR Connection Error: ", err);
    }
  }

  async joinTournament(tournamentId: string) {
    if (this.connection) {
      await this.connection.invoke("JoinTournament", tournamentId);
    }
  }

  async leaveTournament(tournamentId: string) {
    if (this.connection) {
      await this.connection.invoke("LeaveTournament", tournamentId);
    }
  }

  onMatchUpdated(callback: (matchId: string, winnerId: string) => void) {
    if (this.connection) {
      this.connection.on("MatchUpdated", callback);
    }
  }

  onTournamentUpdated(callback: (tournamentId: string) => void) {
    if (this.connection) {
      this.connection.on("TournamentUpdated", callback);
    }
  }

  removeMatchListener() {
    if (this.connection) {
      this.connection.off("MatchUpdated");
    }
  }

  removeTournamentListener() {
    if (this.connection) {
      this.connection.off("TournamentUpdated");
    }
  }
}

export const signalRService = SignalRService.getInstance();
