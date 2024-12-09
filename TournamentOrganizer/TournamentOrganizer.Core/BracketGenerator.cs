﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core
{
    public static class BracketGenerator
    {
        public static IEnumerable<RoundCoreDto> GenerateBracket(
            IEnumerable<ParticipantCoreDto> participants,
            Guid tournamentId
        )
        {
            var participantsList = participants.ToList();
            if (!participantsList.Any())
                throw new ArgumentException(
                    "Must have at least one participant",
                    nameof(participants)
                );

            // Calculate tournament structure
            int totalParticipants = participantsList.Count;
            int totalSlots = (int)BitOperations.RoundUpToPowerOf2((uint)totalParticipants);
            int numberOfByes = totalSlots - totalParticipants;
            int numberOfRounds = (int)Math.Log2(totalSlots);

            var rounds = new List<RoundCoreDto>();
            int matchNumber = 1;

            int GetMatchesForRound(int roundNumber)
            {
                if (roundNumber == 1)
                {
                    return (totalParticipants - numberOfByes) / 2;
                }

                return totalSlots / (int)Math.Pow(2, roundNumber);
            }

            // Round 1: Create matches for players who need to play in first round
            int firstRoundMatches = GetMatchesForRound(1);
            if (firstRoundMatches > 0)
            {
                var firstRoundPlayers = participantsList
                    .Skip(numberOfByes)
                    .Take(firstRoundMatches * 2)
                    .ToList();
                var remainingPlayers = participantsList
                    .Skip(numberOfByes + firstRoundMatches * 2)
                    .ToList();
                var byePlayers = participantsList
                    .Take(numberOfByes)
                    .Concat(remainingPlayers)
                    .ToList();

                var round1Matches = new List<MatchCoreDto>();
                for (int i = 0; i < firstRoundPlayers.Count; i += 2)
                {
                    var match = new MatchCoreDto
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = matchNumber++,
                        Participant1Id = firstRoundPlayers[i].Id,
                        Participant2Id = firstRoundPlayers[i + 1].Id,
                    };
                    round1Matches.Add(match);
                }

                rounds.Add(
                    new RoundCoreDto
                    {
                        Id = Guid.NewGuid(),
                        RoundNumber = 1,
                        TournamentId = tournamentId,
                        Matches = round1Matches,
                    }
                );

                // Update byePlayers list after first round allocation
            }

            // Round 2: Distribute bye players evenly
            int round2Matches = GetMatchesForRound(2);
            var round2MatchList = new List<MatchCoreDto>();
            var byePlayersList = participantsList.Take(numberOfByes).ToList();

            for (int i = 0; i < round2Matches; i++)
            {
                var match = new MatchCoreDto { Id = Guid.NewGuid(), MatchNumber = matchNumber++ };

                // Calculate if this match should receive bye players
                int byePlayersForThisMatch = 0;
                if (byePlayersList.Count > 0)
                {
                    // Distribute bye players evenly from right to left
                    int remainingMatches = round2Matches - i;
                    int remainingByes = byePlayersList.Count;
                    byePlayersForThisMatch = Math.Min(
                        2,
                        (remainingByes + remainingMatches - 1) / remainingMatches
                    );
                }

                if (byePlayersForThisMatch > 0)
                {
                    match.Participant1Id = byePlayersList[0].Id;
                    byePlayersList.RemoveAt(0);

                    if (byePlayersForThisMatch > 1 && byePlayersList.Any())
                    {
                        match.Participant2Id = byePlayersList[0].Id;
                        byePlayersList.RemoveAt(0);
                    }
                }

                round2MatchList.Add(match);
            }

            rounds.Add(
                new RoundCoreDto
                {
                    Id = Guid.NewGuid(),
                    RoundNumber = 2,
                    TournamentId = tournamentId,
                    Matches = round2MatchList,
                }
            );

            // Generate subsequent rounds using the matches formula from GetMatchesForRound
            for (int round = 3; round <= numberOfRounds; round++)
            {
                int matchesInRound = GetMatchesForRound(round);
                var matches = new List<MatchCoreDto>();

                for (int i = 0; i < matchesInRound; i++)
                {
                    matches.Add(
                        new MatchCoreDto
                        {
                            Id = Guid.NewGuid(),
                            MatchNumber = matchNumber++,
                            Participant1Id = null,
                            Participant2Id = null,
                        }
                    );
                }

                rounds.Add(
                    new RoundCoreDto
                    {
                        Id = Guid.NewGuid(),
                        RoundNumber = round,
                        TournamentId = tournamentId,
                        Matches = matches,
                    }
                );
            }

            return rounds;
        }

        public static void UpdateBracket(
            IEnumerable<RoundCoreDto> rounds,
            Guid winnerId,
            Guid matchId
        )
        {
            var roundsList = rounds.ToList();
            var currentRound = roundsList.First(r => r.Matches.Any(m => m.Id == matchId));
            var completedMatch = currentRound.Matches.First(m => m.Id == matchId);
            completedMatch.WinnerId = winnerId;

            var nextRound = roundsList.FirstOrDefault(r =>
                r.RoundNumber == currentRound.RoundNumber + 1
            );
            if (nextRound == null)
                return;

            // Calculate which match in the next round should receive this winner
            int currentMatchIndex = currentRound.Matches.ToList().IndexOf(completedMatch);
            int nextRoundMatchIndex = currentMatchIndex / 2;

            var targetMatch = nextRound.Matches.ElementAt(nextRoundMatchIndex);

            // Check if the target match already has both participants filled
            // If so, find the next available match that can accept a winner
            if (targetMatch.Participant1Id != null && targetMatch.Participant2Id != null)
            {
                targetMatch = nextRound.Matches.FirstOrDefault(m =>
                    m.Participant1Id == null || m.Participant2Id == null
                );
            }

            if (targetMatch == null)
                return;

            // Determine correct slot based on current match number
            if (currentMatchIndex % 2 == 0)
            {
                if (targetMatch.Participant1Id == null)
                    targetMatch.Participant1Id = winnerId;
                else if (targetMatch.Participant2Id == null)
                    targetMatch.Participant2Id = winnerId;
            }
            else
            {
                if (targetMatch.Participant2Id == null)
                    targetMatch.Participant2Id = winnerId;
                else if (targetMatch.Participant1Id == null)
                    targetMatch.Participant1Id = winnerId;
            }
        }
    }
}
