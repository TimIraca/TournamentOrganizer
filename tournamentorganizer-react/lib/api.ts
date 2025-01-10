import axios from "axios";
import {
  Match,
  Participant,
  Round,
  type Tournament,
  type TournamentOverview,
  editTournament,
} from "@/types";

const api = axios.create({
  baseURL: "/api",
  timeout: 30000,
  headers: {
    "Content-Type": "application/json",
  },
});
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  if (process.env.NODE_ENV === "development") {
    console.log("API Request URL:", `${config.baseURL}${config.url}`);
  }
  return config;
});
// Add response interceptor for better error handling
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Handle unauthorized errors (invalid or expired token)
    if (error.response?.status === 401) {
      localStorage.removeItem("token");
      window.location.href = "/auth/login";
    }

    if (process.env.NODE_ENV === "development") {
      console.error("API Error:", {
        message: error.message,
        url: error.config?.url,
        method: error.config?.method,
        status: error.response?.status,
        data: error.response?.data,
      });
    }
    return Promise.reject(error);
  }
);
export const tournamentApi = {
  // Tournament endpoints
  getAllTournaments: async () => {
    const response = await api.get<Tournament[]>("/Tournaments");
    return response.data;
  },
  getTournament: async (id: string) => {
    const response = await api.get<Tournament>(`Tournaments/${id}`);
    return response.data;
  },
  updateTournament: async (id: string, tournament: editTournament) => {
    await api.put(`/Tournaments/${id}`, tournament);
  },
  createTournament: async (tournament: editTournament) => {
    await api.post("/Tournaments", tournament);
  },
  startTournament: async (id: string) => {
    await api.post(`/Tournaments/${id}/start`);
  },
  resetTournament: async (id: string) => {
    await api.post(`/Tournaments/${id}/reset`);
  },
  deleteTournament: async (id: string) => {
    try {
      const response = await api.delete(`/Tournaments/${id}`);
      return;
    } catch (error) {
      throw error;
    }
  },
  getTournamentOverview: async (id: string) => {
    const response = await api.get<TournamentOverview>(
      `/Overview/tournament/${id}`
    );
    return response.data;
  },
  getRounds: async (tournamentId: string) => {
    const response = await api.get<Round[]>(
      `tournaments/${tournamentId}/rounds`
    );
    return response.data;
  },
  getMatchesByRound: async (id: string, tournamentId: string) => {
    const response = await api.get<Match[]>(
      `tournaments/${tournamentId}/matches/round/${id}`
    );
    return response.data;
  },
  declareMatchWinner: async (
    tournamentId: string,
    winnerId: string,
    matchId: string
  ) => {
    await api.post(`tournaments/${tournamentId}/matches/${matchId}/winner`, {
      winnerId,
    });
  },

  getParticipants: async (id: string) => {
    const response = await api.get<Participant[]>(
      `tournaments/${id}/participants`
    );
    return response.data;
  },
  addParticipant: async (id: string, name: string) => {
    const response = await api.post<Participant>(
      `tournaments/${id}/participants`,
      { name }
    );
    return response.data;
  },
  updateParticipant: async (
    tournamentId: string,
    participantId: string,
    name: string
  ) => {
    await api.put(`tournaments/${tournamentId}/participants/${participantId}`, {
      name,
    });
  },
  deleteParticipant: async (tournamentId: string, participantId: string) => {
    await api.delete(
      `tournaments/${tournamentId}/participants/${participantId}`
    );
  },
};

export const handleApiError = (error: unknown) => {
  if (axios.isAxiosError(error)) {
    if (error.code === "ECONNREFUSED") {
      return "Cannot connect to the API. Please ensure the server is running.";
    }
    if (error.response?.status === 401) {
      return "Please log in to continue.";
    }
    if (error.response?.status === 400) {
      return "Invalid request. Please check your input.";
    }
    if (error.response?.status === 404) {
      return "Tournament not found.";
    }
    if (error.response?.status === 500) {
      return "Server error. Please try again later.";
    }
    if (!error.response) {
      return "Network error. Please check your connection.";
    }
  }
  return "An unexpected error occurred. Please try again.";
};
