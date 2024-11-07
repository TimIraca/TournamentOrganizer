// lib/api.ts
import axios from 'axios';
import type {
    Tournament,
    Participant,
    Match,
    CreateTournamentRequest,
    RegisterParticipantRequest,
    UpdateMatchScoreRequest
} from '@/types';

const api = axios.create({
    baseURL: 'https://localhost:7180/api/' // Set your API link here
});

export const tournamentApi = {
    // Tournament endpoints
    getAllTournaments: async () => {
        const response = await api.get<Tournament[]>('/Tournament');
        return response.data;
    },

    getTournament: async (id: string) => {
        const response = await api.get<Tournament>(`/tournaments/${id}`);
        return response.data;
    },

    createTournament: async (data: CreateTournamentRequest) => {
        const response = await api.post<Tournament>('/tournaments', data);
        return response.data;
    },

    // Participant endpoints
    getParticipants: async (tournamentId: string) => {
        const response = await api.get<Participant[]>(`/Participant/tournament/${tournamentId}`);
        return response.data;
    },

    registerParticipant: async (tournamentId: string, data: RegisterParticipantRequest) => {
        const response = await api.post<Participant>(`/tournaments/${tournamentId}/participants`, data);
        return response.data;
    },

    removeParticipant: async (tournamentId: string, participantId: string) => {
        await api.delete(`/tournaments/${tournamentId}/participants/${participantId}`);
    },

    // Match endpoints
    getMatches: async (tournamentId: string) => {
        const response = await api.get<Match[]>(`/tournaments/${tournamentId}/matches`);
        return response.data;
    },

    updateMatchScore: async (
        tournamentId: string,
        matchId: string,
        data: UpdateMatchScoreRequest
    ) => {
        await api.put(`/tournaments/${tournamentId}/matches/${matchId}/score`, data);
    },

    // Tournament management
    startTournament: async (tournamentId: string) => {
        await api.post(`/tournaments/${tournamentId}/start`);
    },

    endTournament: async (tournamentId: string) => {
        await api.post(`/tournaments/${tournamentId}/end`);
    }
};

export const handleApiError = (error: unknown) => {
    if (axios.isAxiosError(error)) {
        if (error.response?.status === 400) {
            return "Invalid request. Please check your input.";
        }
        if (error.response?.status === 404) {
            return "Tournament not found.";
        }
    }
    return "An unexpected error occurred. Please try again.";
};