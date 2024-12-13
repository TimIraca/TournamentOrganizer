"use client";
import { useState, useEffect } from "react";
import { TournamentCard } from "@/components/Tournaments/TournamentCard";
import { editTournament, Tournament } from "@/types";
import { Edit, Trash } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { SidebarTrigger } from "@/components/ui/sidebar";
import { tournamentApi } from "@/lib/api";
import { ModeToggle } from "@/components/modetoggle";
import Link from "next/link";
import { GeneralContextMenu } from "@/components/general-context-menu";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import { CreateTournamentDialog } from "./create-tournament-dialog";

export default function Page() {
  const [tournaments, setTournaments] = useState<Tournament[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [tournamentToDelete, setTournamentToDelete] = useState<string | null>(
    null
  );
  const handleDeleteConfirm = async () => {
    if (tournamentToDelete) {
      try {
        await tournamentApi.deleteTournament(tournamentToDelete);
        setTournaments(tournaments.filter((t) => t.id !== tournamentToDelete));
        setTournamentToDelete(null);
      } catch (err) {
        setError(`Failed to delete tournament: ${err}`);
      }
    }
  };
  const handleCreateTournament = async (newTournament: editTournament) => {
    try {
      await tournamentApi.createTournament(newTournament);
    } catch (err) {
      setError(`Failed to create tournament: ${err}`);
    } finally {
      fetchTournaments();
    }
  };
  const fetchTournaments = async () => {
    try {
      const data = await tournamentApi.getAllTournaments();
      setTournaments(data);
    } catch {
      setError("Failed to create tournament:");
    } finally {
      setLoading(false);
    }
  };
  useEffect(() => {
    fetchTournaments();
  }, []);
  return (
    <div className="flex flex-col gap-6 p-6 w-full">
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-4">
          <SidebarTrigger />
          <ModeToggle />
        </div>
        <h1 className="text-3xl font-bold tracking-tight text-center flex-grow">
          Your Tournaments
        </h1>

        <CreateTournamentDialog onCreateTournament={handleCreateTournament} />
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
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {tournaments.map((tournament) => (
            <GeneralContextMenu
              key={tournament.id}
              data-cy={tournament.id}
              actions={[
                {
                  label: "Edit",
                  icon: <Edit className="h-4 w-4" />,
                  onClick: (e) => {
                    e.preventDefault();
                    e.stopPropagation();
                  },
                  href: `/tournaments/${tournament.id}/edit`,
                },
                {
                  label: "Delete",
                  icon: <Trash className="h-4 w-4" />,
                  onClick: (e) => {
                    e.preventDefault();
                    e.stopPropagation();
                    setTournamentToDelete(tournament.id);
                  },
                },
              ]}
            >
              <Link
                href={`/tournaments/${tournament.id}`}
                className="block h-full"
              >
                <TournamentCard
                  tournament={tournament}
                  data-cy={tournament.id}
                />
              </Link>
            </GeneralContextMenu>
          ))}
        </div>
      )}
      <AlertDialog
        open={!!tournamentToDelete}
        onOpenChange={() => setTournamentToDelete(null)}
      >
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
            <AlertDialogDescription>
              This action cannot be undone. This will permanently delete the
              tournament and remove all associated data from our servers.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction onClick={handleDeleteConfirm}>
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}
