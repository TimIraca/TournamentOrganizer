// types/index.ts
export enum TournamentFormat {
    SingleElimination = 0,
    DoubleElimination = 1,
    RoundRobin = 2
}

export type TournamentStatus = 'Registration' | 'InProgress' | 'Completed';
export type MatchStatus = 'Pending' | 'InProgress' | 'Completed';

export interface Tournament {
    id: string;
    name: string;
    description: string;
    format: TournamentFormat;  // Changed to enum
    status: TournamentStatus;
    startDate: string;
    maxParticipants: number;
    currentParticipants: number;
    prizePool: number;
    prizeCurrency: string;
}

export interface Participant {
    id: string;
    participantName: string;
    registrationDate: string;
}

export interface Match {
    id: string;
    round: number;
    participant1: Participant;
    participant2: Participant;
    participant1Score: number | null;
    participant2Score: number | null;
    status: MatchStatus;
    scheduledTime: string | null;
}

export interface CreateTournamentRequest {
    name: string;
    description?: string;
    format: TournamentFormat;  // Changed to enum
    startDate: string;
    maxParticipants: number;
    prizePool?: number;
    prizeCurrency?: string;
}

export interface RegisterParticipantRequest {
    participantName: string;
}

export interface UpdateMatchScoreRequest {
    participant1Score: number;
    participant2Score: number;
}