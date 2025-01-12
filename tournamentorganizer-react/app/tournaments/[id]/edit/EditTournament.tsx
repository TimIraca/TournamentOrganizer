"use client";

import { useState, useEffect } from "react";
import { format } from "date-fns";
import { useRouter } from "next/navigation";
import { CalendarIcon, Loader2, Trash2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Calendar } from "@/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { toast } from "@/hooks/use-toast";
import { cn } from "@/lib/utils";
import { Tournament } from "@/types";
import { useParams } from "next/navigation";
import { tournamentApi } from "@/lib/api";
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

export interface EditTournament {
  name: string;
  startDate: string;
}

type DialogType = "delete" | "start" | "reset" | null;

interface DialogConfig {
  title: string;
  description: string;
  actionText: string;
  action: () => Promise<void>;
}

export default function EditTournament() {
  const params = useParams();
  const tournamentId = params?.id;
  const [tournament, setTournament] = useState<Tournament | null>(null);
  const [name, setName] = useState("");
  const [startDate, setStartDate] = useState<Date | null>(null);
  const [activeDialog, setActiveDialog] = useState<DialogType>(null);
  const [loading, setLoading] = useState(true);
  const router = useRouter();

  useEffect(() => {
    const loadTournament = async () => {
      try {
        const data = await tournamentApi.getTournament(tournamentId as string);
        setTournament(data);
        setName(data.name);
        const parsedDate = new Date(data.startDate);
        if (!isNaN(parsedDate.getTime())) {
          setStartDate(parsedDate);
        }
        setLoading(false);
      } catch {
        toast({
          title: "Error",
          description: "Failed to load tournament. Please try again.",
          variant: "destructive",
        });
      }
    };
    loadTournament();
  }, [tournamentId]);

  const handleUpdateTournament = async () => {
    if (!startDate || !name.trim()) {
      toast({
        title: "Error",
        description: "Name and Start Date are required.",
        variant: "destructive",
      });
      return;
    }

    const updatedTournament: EditTournament = {
      name,
      startDate: new Date(
        Date.UTC(
          startDate.getFullYear(),
          startDate.getMonth(),
          startDate.getDate()
        )
      ).toISOString(),
    };

    try {
      await tournamentApi.updateTournament(
        tournamentId as string,
        updatedTournament
      );
      toast({
        title: "Tournament updated",
        description: `The tournament has been updated successfully.`,
      });
    } catch {
      toast({
        title: "Error",
        description: "Failed to update tournament. Please try again.",
        variant: "destructive",
      });
    }
  };

  const handleDeleteConfirm = async () => {
    if (tournament) {
      try {
        await tournamentApi.deleteTournament(tournament.id);
        toast({
          title: "Tournament deleted",
          description: "The tournament has been successfully deleted.",
        });
        router.push("/tournaments");
      } catch (err) {
        console.error("Error deleting tournament:", err);
        toast({
          title: "Error",
          description: "Failed to delete tournament. Please try again.",
          variant: "destructive",
        });
      } finally {
        setActiveDialog(null);
      }
    }
  };

  const handleStartConfirm = async () => {
    if (tournament) {
      try {
        await tournamentApi.startTournament(tournament.id);
        toast({
          title: "Tournament started",
          description: "The tournament has been successfully started.",
        });
        router.refresh();
      } catch (err) {
        console.error("Error starting tournament:", err);
        toast({
          title: "Error",
          description: "Failed to start tournament. Please try again.",
          variant: "destructive",
        });
      } finally {
        setActiveDialog(null);
      }
    }
  };

  const handleResetConfirm = async () => {
    if (tournament) {
      try {
        await tournamentApi.resetTournament(tournament.id);
        toast({
          title: "Tournament reset",
          description: "The tournament has been successfully reset.",
        });
        router.refresh();
      } catch (err) {
        console.error("Error resetting tournament:", err);
        toast({
          title: "Error",
          description: "Failed to reset tournament. Please try again.",
          variant: "destructive",
        });
      } finally {
        setActiveDialog(null);
      }
    }
  };

  const dialogConfigs: Record<NonNullable<DialogType>, DialogConfig> = {
    delete: {
      title: "Delete Tournament",
      description:
        "This action cannot be undone. This will permanently delete the tournament and remove all associated data from our servers.",
      actionText: "Delete",
      action: handleDeleteConfirm,
    },
    start: {
      title: "Start Tournament",
      description:
        "This will start the tournament and generate the initial bracket. Make sure all participants are added before starting. This action cannot be undone.",
      actionText: "Start",
      action: handleStartConfirm,
    },
    reset: {
      title: "Reset Tournament",
      description:
        "This will reset all matches and rounds. All match results will be permanently lost. This action cannot be undone.",
      actionText: "Reset",
      action: handleResetConfirm,
    },
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Loader2 className="h-8 w-8 animate-spin" />
      </div>
    );
  }

  const activeDialogConfig = activeDialog ? dialogConfigs[activeDialog] : null;

  return (
    <>
      <Card className="w-full max-w-2xl mx-auto relative">
        <Button
          data-cy="delete-tournament-button"
          variant="destructive"
          size="icon"
          className="absolute top-4 right-4"
          onClick={() => setActiveDialog("delete")}
        >
          <Trash2 className="h-4 w-4" />
        </Button>
        <CardHeader>
          <CardTitle>Edit Tournament Details</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="tournamentName">Tournament Name</Label>
              <Input
                data-cy="tournament-name-input"
                id="tournamentName"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Unnamed Tournament"
              />
            </div>
            <div className="space-y-2">
              <Label>Start Date</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={"outline"}
                    className={cn(
                      "w-full justify-start text-left font-normal",
                      !startDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {startDate && !isNaN(startDate.getTime()) ? (
                      format(startDate, "PPP")
                    ) : (
                      <span>Pick a date</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0">
                  <Calendar
                    mode="single"
                    selected={startDate || undefined}
                    onSelect={(date) => {
                      if (date) {
                        date.setHours(0, 0, 0, 0);
                        setStartDate(date);
                      } else {
                        setStartDate(null);
                      }
                    }}
                    initialFocus
                  />
                </PopoverContent>
              </Popover>
            </div>
            <div className="flex justify-between">
              <Button variant="default" onClick={handleUpdateTournament}>
                Save Changes
              </Button>
              <Button
                data-cy="start-tournament-button"
                variant="default"
                onClick={() => setActiveDialog("start")}
              >
                Start Tournament
              </Button>
              <Button
                variant="outline"
                onClick={() => setActiveDialog("reset")}
              >
                Reset Tournament
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>

      <AlertDialog
        open={activeDialog !== null}
        onOpenChange={(open) => !open && setActiveDialog(null)}
      >
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>{activeDialogConfig?.title}</AlertDialogTitle>
            <AlertDialogDescription>
              {activeDialogConfig?.description}
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              data-cy="confirm-delete-button"
              onClick={activeDialogConfig?.action}
            >
              {activeDialogConfig?.actionText}
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </>
  );
}
