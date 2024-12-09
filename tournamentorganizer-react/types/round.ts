import { Match } from "./match";
import { MatchOverview } from "./match";

export interface Round {
  id: string;
  roundNumber: number;
  matches: Match[];
}
export interface RoundOverview {
  id: string;
  roundNumber: number;
  matches: MatchOverview[];
}
