// lib/api.ts
import axios from "axios";
import type {
  Tournament,
  Participant,
  Match,
  CreateTournamentRequest,
  RegisterParticipantRequest,
  UpdateMatchScoreRequest,
} from "@/types";

// Get the API URL from environment variables with a fallback
const API_URL =
  process.env.NEXT_PUBLIC_API_URL || "http://tournamentorganizer.api:8080/api";

const api = axios.create({
  baseURL: API_URL,
  timeout: 30000,
  headers: {
    "Content-Type": "application/json",
  },
  // Since we're in development, we can disable SSL verification
  // Remove this in production
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  httpsAgent: new (require("https").Agent)({
    rejectUnauthorized: false,
  }),
});

// Add request interceptor for Docker network handling
api.interceptors.request.use((config) => {
  // Log the request URL in development
  if (process.env.NODE_ENV === "development") {
    console.log("API Request URL:", `${config.baseURL}${config.url}`);
  }
  return config;
});

// Add response interceptor for better error handling
api.interceptors.response.use(
  (response) => response,
  (error) => {
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
    const response = await api.get<Tournament[]>("/Tournament");
    return response.data;
  },

  getTournament: async (id: string) => {
    const response = await api.get<Tournament>(`/Tournament/${id}`);
    return response.data;
  },

  createTournament: async (data: CreateTournamentRequest) => {
    const response = await api.post<Tournament>("/Tournament", data);
    return response.data;
  },

  // Participant endpoints
  getParticipants: async (tournamentId: string) => {
    const response = await api.get<Participant[]>(
      `/Participant/tournament/${tournamentId}`
    );
    return response.data;
  },

  registerParticipant: async (
    tournamentId: string,
    data: RegisterParticipantRequest
  ) => {
    const response = await api.post<Participant>(
      `/Participant/tournament/${tournamentId}`,
      data
    );
    return response.data;
  },

  removeParticipant: async (tournamentId: string, participantId: string) => {
    await api.delete(
      `/Participant/${participantId}/tournament/${tournamentId}`
    );
  },

  // Match endpoints
  getMatches: async (tournamentId: string) => {
    const response = await api.get<Match[]>(
      `/Match/tournament/${tournamentId}`
    );
    return response.data;
  },

  updateMatchScore: async (
    tournamentId: string,
    matchId: string,
    data: UpdateMatchScoreRequest
  ) => {
    await api.put(`/Match/${matchId}/tournament/${tournamentId}/score`, data);
  },

  // Tournament management
  startTournament: async (tournamentId: string) => {
    await api.post(`/Tournament/${tournamentId}/start`);
  },

  endTournament: async (tournamentId: string) => {
    await api.post(`/Tournament/${tournamentId}/end`);
  },
};

export const handleApiError = (error: unknown) => {
  if (axios.isAxiosError(error)) {
    if (error.code === "ECONNREFUSED") {
      return "Cannot connect to the API. Please ensure the server is running.";
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
