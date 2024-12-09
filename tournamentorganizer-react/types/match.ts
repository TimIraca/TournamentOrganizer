import { Participant } from "./participant";

export interface Match {
  id: string;
  matchNumber: number;
  participants: Participant[];
  winnerId: string | null;
  participant1Id: string;
  participant2Id: string;
}
export interface MatchOverview {
  id: string;
  matchNumber: number;
  participant1Name: string;
  participant2Name: string;
  winnerName: string;
}
