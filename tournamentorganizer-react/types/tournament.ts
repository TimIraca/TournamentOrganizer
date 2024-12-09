import { Participant, ParticipantOverview } from "./participant";
import { Round, RoundOverview } from "./round";

export interface Tournament {
  id: string;
  name: string;
  startDate: string;
  isCompleted: boolean;
  rounds: Round[];
  participants: Participant[];
}
export interface editTournament {
  name: string;
  startDate: string;
}
export interface TournamentOverview {
  id: string;
  name: string;
  startDate: string;
  isCompleted: boolean;
  rounds: RoundOverview[];
  participants: ParticipantOverview[];
}
