"use client";
// components/layout/MainLayout.tsx
import { useEffect, useState } from "react";
import {
  Badge,
  HelpCircle,
  MessageSquare,
  PlusCircle,
  Trophy,
  Users,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  SidebarProvider,
  Sidebar,
  SidebarInset,
  SidebarFooter,
} from "@/components/ui/sidebar";
import { AppHeader } from "./AppHeader";
import { SidebarNav } from "./SidebarNav";
import { UserProfile } from "./UserProfile";
import { TournamentCard } from "../features/tournaments/TournamentCard";
import { TournamentForm } from "../features/tournaments/TournamentForm";
import { TournamentDetails } from "../features/tournaments/TournamentDetails";
import { ParticipantList } from "../features/tournaments/ParticipantList";
import { Dialog, DialogContent, DialogTrigger } from "@/components/ui/dialog";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { Skeleton } from "@/components/ui/skeleton";
import { tournamentApi, handleApiError } from "@/lib/api";
import type {
  Tournament,
  Participant,
  CreateTournamentRequest,
  Match,
} from "@/types";

// Move to separate file (lib/constants.ts)
const navItems = {
  navMain: [
    {
      title: "Tournaments",
      url: "#",
      icon: Trophy,
      isActive: true,
      items: [
        { title: "Active", url: "#" },
        { title: "Registration", url: "#" },
        { title: "Completed", url: "#" },
      ],
    },
    {
      title: "Participants",
      url: "#",
      icon: Users,
      items: [
        { title: "Manage", url: "#" },
        { title: "Registrations", url: "#" },
      ],
    },
    // other
  ],
  navSecondary: [
    { title: "Help Center", url: "#", icon: HelpCircle },
    { title: "Feedback", url: "#", icon: MessageSquare },
  ],
  user: {
    name: "Sarah Johnson",
    email: "sarah@tournamentorganizer.com",
    avatar: "/placeholder.svg?height=32&width=32",
  },
};

export function MainLayout() {
  const [tournaments, setTournaments] = useState<Tournament[]>([]);
  const [selectedTournament, setSelectedTournament] =
    useState<Tournament | null>(null);
  const [participants, setParticipants] = useState<Participant[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);

  useEffect(() => {
    fetchTournaments();
  }, []);

  useEffect(() => {
    if (selectedTournament) {
      fetchParticipants(selectedTournament.id);
    }
  }, [selectedTournament]);

  const fetchTournaments = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await tournamentApi.getAllTournaments();
      setTournaments(data);
      if (!selectedTournament && data.length > 0) {
        setSelectedTournament(data[0]);
      }
    } catch (err) {
      setError(handleApiError(err));
    } finally {
      setLoading(false);
    }
  };

  const fetchParticipants = async (tournamentId: string) => {
    try {
      const data = await tournamentApi.getParticipants(tournamentId);
      setParticipants(data);
    } catch (err) {
      console.error("Failed to fetch participants:", err);
    }
  };

  const handleCreateTournament = async (data: CreateTournamentRequest) => {
    try {
      const newTournament = await tournamentApi.createTournament(data);
      setTournaments((prev) => [...prev, newTournament]);
      setIsCreateDialogOpen(false);
    } catch (err) {
      setError(handleApiError(err));
    }
  };

  const handleRemoveParticipant = async (
    tournamentId: string,
    participantId: string
  ) => {
    try {
      await tournamentApi.removeParticipant(participantId);
      await fetchParticipants(tournamentId);
    } catch (err) {
      setError(handleApiError(err));
    }
  };

  const handleStartTournament = async (tournamentId: string) => {
    try {
      await tournamentApi.startTournament(tournamentId);
      await fetchTournaments();
    } catch (err) {
      setError(handleApiError(err));
    }
  };

  return (
    <SidebarProvider>
      <Sidebar variant="inset">
        <SidebarNav navItems={navItems.navMain} />
        <SidebarFooter>
          <UserProfile user={navItems.user} />
        </SidebarFooter>
      </Sidebar>
      <SidebarInset>
        <AppHeader />
        <div className="flex flex-col gap-6 p-6">
          <div className="flex items-center justify-between">
            <h1 className="text-3xl font-bold tracking-tight">Tournaments</h1>
            <Dialog
              open={isCreateDialogOpen}
              onOpenChange={setIsCreateDialogOpen}
            >
              <DialogTrigger asChild>
                <Button>
                  <PlusCircle className="mr-2 h-4 w-4" />
                  New Tournament
                </Button>
              </DialogTrigger>
              <DialogContent className="sm:max-w-[425px]">
                <TournamentForm onSubmit={handleCreateTournament} />
              </DialogContent>
            </Dialog>
          </div>

          {error && (
            <Alert variant="destructive">
              <AlertDescription>{error}</AlertDescription>
            </Alert>
          )}

          {loading ? (
            <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
              {[1, 2, 3].map((n) => (
                <div key={n} className="space-y-3">
                  <Skeleton className="h-[125px] w-full rounded-xl" />
                </div>
              ))}
            </div>
          ) : (
            <>
              <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                {tournaments.map((tournament) => (
                  <TournamentCard
                    key={tournament.id}
                    tournament={tournament}
                    onClick={setSelectedTournament}
                  />
                ))}
              </div>

              {selectedTournament && (
                <div className="grid gap-6 lg:grid-cols-2">
                  <TournamentDetails
                    tournament={selectedTournament}
                    onStartTournament={() =>
                      handleStartTournament(selectedTournament.id)
                    }
                  />
                  <div className="space-y-6">
                    <ParticipantList
                      participants={participants}
                      maxParticipants={selectedTournament.maxParticipants}
                      onRemoveParticipant={
                        selectedTournament.status === "Registration"
                          ? (participantId) =>
                              handleRemoveParticipant(
                                selectedTournament.id,
                                participantId
                              )
                          : undefined
                      }
                    />
                    {selectedTournament.status === "InProgress" && (
                      <MatchList tournamentId={selectedTournament.id} />
                    )}
                  </div>
                </div>
              )}
            </>
          )}
        </div>
      </SidebarInset>
    </SidebarProvider>
  );
}

// components/features/tournaments/MatchList.tsx
interface MatchListProps {
  tournamentId: string;
}

function MatchList({ tournamentId }: MatchListProps) {
  const [matches, setMatches] = useState<Match[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchMatches = async () => {
      try {
        setLoading(true);
        const data = await tournamentApi.getMatches(tournamentId);
        setMatches(data);
      } catch (err) {
        console.error("Failed to fetch matches:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchMatches();
  }, [tournamentId]);

  if (loading) {
    return <Skeleton className="h-[200px] w-full" />;
  }

  return (
    <div className="rounded-lg border">
      <div className="p-4 border-b">
        <h3 className="text-lg font-medium">Matches</h3>
      </div>
      <div className="divide-y">
        {matches.map((match) => (
          <div key={match.id} className="p-4">
            <div className="flex items-center justify-between">
              <div className="flex-1">
                <p className="font-medium">
                  {match.participant1.participantName}
                </p>
                <p className="text-sm text-muted-foreground">vs</p>
                <p className="font-medium">
                  {match.participant2.participantName}
                </p>
              </div>
              <div className="text-right">
                <p className="text-sm font-medium">
                  {match.participant1Score ?? "-"} -{" "}
                  {match.participant2Score ?? "-"}
                </p>
                <Badge>{match.status}</Badge>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
