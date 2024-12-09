// schemas/tournamentSchema.ts
import { z } from "zod";

export enum TournamentFormat {
  SingleElimination = 0,
  DoubleElimination = 1,
  RoundRobin = 2,
}

// Create a mapping for user-friendly labels
export const TournamentFormatLabels: Record<TournamentFormat, string> = {
  [TournamentFormat.SingleElimination]: "Single Elimination",
  [TournamentFormat.DoubleElimination]: "Double Elimination",
  [TournamentFormat.RoundRobin]: "Round Robin",
};

export const createTournamentSchema = z.object({
  name: z.string().min(1, "Name is required"),
  description: z.string().optional(),
  format: z.nativeEnum(TournamentFormat, {
    errorMap: () => ({ message: "Invalid format" }),
  }),
  startDate: z.string().min(1, "Start date is required"),
  maxParticipants: z.number().min(1, "Must have at least 1 participant"),
  prizePool: z.number().optional(),
  prizeCurrency: z.string().optional(),
});

export type CreateTournamentRequest = z.infer<typeof createTournamentSchema>;
