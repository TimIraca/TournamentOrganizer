"use client";

import { useEffect, useState } from "react";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area";
import { Calendar, CheckCircle, Clock, Loader2, Trophy } from "lucide-react";
import { Tournament, Round, Match, Participant } from "@/types/";
import { Separator } from "@/components/ui/separator";
import { tournamentApi } from "@/lib/api";
import React from "react";
import { GeneralDropdownMenu } from "@/components/general-dropdown-menu";

interface TournamentBracketProps {
  tournamentId: string;
}

export function TournamentBracket({ tournamentId }: TournamentBracketProps) {
  const [tournament, setTournament] = useState<Tournament | null>(null);
  const [loading, setLoading] = useState(true);
  const [highlightedParticipantId, setHighlightedParticipantId] = useState<
    string | null
  >(null);

  const fetchTournamentData = async () => {
    try {
      const tournamentDetails = await tournamentApi.getTournament(tournamentId);
      const rounds = await tournamentApi.getRounds(tournamentId);

      const enrichedRounds = await Promise.all(
        rounds.map(async (round) => {
          const matches = await tournamentApi.getMatchesByRound(
            round.id,
            tournamentId
          );

          const enrichedMatches: Match[] = matches.map((match) => ({
            ...match,
            participants: [
              tournamentDetails.participants.find(
                (p) => p.id === match.participant1Id
              ),
              tournamentDetails.participants.find(
                (p) => p.id === match.participant2Id
              ),
            ].filter(
              (participant): participant is Participant =>
                participant !== undefined
            ),
          }));

          return { ...round, matches: enrichedMatches };
        })
      );

      const fullTournamentData: Tournament = {
        ...tournamentDetails,
        rounds: enrichedRounds,
      };

      setTournament(fullTournamentData);
    } catch (error) {
      console.error("Error fetching tournament, rounds, or matches:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (tournamentId) {
      fetchTournamentData();
    }
  }, [tournamentId]);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Loader2 className="h-8 w-8 animate-spin" />
      </div>
    );
  }

  if (!tournament) {
    return <div>Error loading tournament details.</div>;
  }

  if (tournament.rounds.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center h-64 text-center">
        <p className="text-lg font-semibold mb-2">Tournament not yet started</p>
        <p className="text-sm text-muted-foreground">
          Go to edit tournament to add participants and start the tournament
        </p>
      </div>
    );
  }

  return (
    <ScrollArea className="w-full whitespace-nowrap rounded-md border">
      <div className="flex space-x-4 p-4">
        {tournament.rounds
          .sort((a, b) => a.roundNumber - b.roundNumber)
          .map((round) => (
            <RoundColumn
              key={round.roundNumber}
              round={round}
              highlightedParticipantId={highlightedParticipantId}
              setHighlightedParticipantId={setHighlightedParticipantId}
              tournamentId={tournamentId}
              onMatchUpdate={fetchTournamentData}
            />
          ))}
      </div>
      <ScrollBar orientation="horizontal" />
    </ScrollArea>
  );
}

function RoundColumn({
  round,
  highlightedParticipantId,
  setHighlightedParticipantId,
  tournamentId,
  onMatchUpdate,
}: {
  round: Round;
  highlightedParticipantId: string | null;
  setHighlightedParticipantId: (id: string | null) => void;
  tournamentId: string;
  onMatchUpdate: () => Promise<void>;
}) {
  return (
    <div className="flex flex-col space-y-4">
      <h2 className="text-xl font-semibold text-center">
        Round {round.roundNumber}
      </h2>
      {round.matches
        .sort((a, b) => a.matchNumber - b.matchNumber)
        .map((match) => (
          <MatchCard
            key={match.id}
            match={match}
            highlightedParticipantId={highlightedParticipantId}
            setHighlightedParticipantId={setHighlightedParticipantId}
            tournamentId={tournamentId}
            onMatchUpdate={onMatchUpdate}
          />
        ))}
    </div>
  );
}

function MatchCard({
  match,
  highlightedParticipantId,
  setHighlightedParticipantId,
  tournamentId,
  onMatchUpdate,
}: {
  match: Match;
  highlightedParticipantId: string | null;
  setHighlightedParticipantId: (id: string | null) => void;
  tournamentId: string;
  onMatchUpdate: () => Promise<void>;
}) {
  const [isUpdating, setIsUpdating] = useState(false);

  const handleDeclareWinner = async (participantId: string) => {
    setIsUpdating(true);
    try {
      await tournamentApi.declareMatchWinner(
        tournamentId,
        participantId,
        match.id
      );
      await onMatchUpdate();
    } catch (error) {
      console.error("Error declaring winner:", error);
      console.error("Failed to declare winner for match:", match);
    } finally {
      setIsUpdating(false);
    }
  };

  const renderTBD = () => (
    <div className="flex items-center justify-between p-2">
      <span className="font-bold text-red-500">TBD</span>
    </div>
  );

  if (isUpdating) {
    return (
      <Card className="w-64">
        <CardContent className="p-4 flex justify-center items-center h-32">
          <Loader2 className="h-6 w-6 animate-spin" />
        </CardContent>
      </Card>
    );
  }

  return (
    <Card className="w-64">
      <CardContent className="p-4">
        {match.participants.length === 0 ? (
          <>
            {renderTBD()}
            <Separator className="my-2" />
            {renderTBD()}
          </>
        ) : (
          <>
            {match.participants.map((participant, index) => {
              const isEligibleForContextMenu =
                match.participants.length === 2 && !match.winnerId;

              const actions = [
                {
                  label: "Declare Winner",
                  icon: <Trophy className="w-4 h-4 mr-1" />,
                  onClick: () => handleDeclareWinner(participant.id),
                },
              ];

              const participantContent = (
                <div
                  key={participant.id}
                  onMouseEnter={() =>
                    setHighlightedParticipantId(participant.id)
                  }
                  onMouseLeave={() => setHighlightedParticipantId(null)}
                  className={`flex items-center justify-between p-2 rounded ${
                    highlightedParticipantId === participant.id
                      ? "animate-outline-pulse"
                      : ""
                  } ${isEligibleForContextMenu ? "cursor-pointer" : ""}`}
                >
                  <span>{participant.name}</span>
                  {match.winnerId === participant.id && (
                    <Badge variant="secondary" className="bg-winner">
                      <Trophy className="w-4 h-4 mr-1" />
                      Winner
                    </Badge>
                  )}
                </div>
              );

              return (
                <React.Fragment key={participant.id}>
                  {isEligibleForContextMenu ? (
                    <GeneralDropdownMenu actions={actions}>
                      {participantContent}
                    </GeneralDropdownMenu>
                  ) : (
                    participantContent
                  )}
                  {index === 0 && match.participants.length === 2 && (
                    <Separator className="my-2" />
                  )}
                </React.Fragment>
              );
            })}
            {match.participants.length === 1 && (
              <>
                <Separator className="my-2" />
                {renderTBD()}
              </>
            )}
          </>
        )}
        <div className="mt-2 flex justify-center">
          {match.participants.length === 0 && (
            <Badge variant="secondary" className="bg-blue-100 text-blue-800">
              <Calendar className="w-4 h-4 mr-1" />
              Not Started
            </Badge>
          )}
          {match.participants.length === 1 && (
            <Badge variant="secondary" className="bg-blue-100 text-blue-800">
              <Calendar className="w-4 h-4 mr-1" />
              Upcoming
            </Badge>
          )}
          {match.participants.length === 2 && !match.winnerId && (
            <Badge
              variant="secondary"
              className="bg-yellow-100 text-yellow-800"
            >
              <Clock className="w-4 h-4 mr-1" />
              In Progress
            </Badge>
          )}
          {match.participants.length === 2 && match.winnerId && (
            <Badge variant="secondary" className="bg-green-100 text-green-800">
              <CheckCircle className="w-4 h-4 mr-1" />
              Completed
            </Badge>
          )}
        </div>
      </CardContent>
    </Card>
  );
}
