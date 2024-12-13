"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { toast } from "@/hooks/use-toast";

import { editTournament } from "@/types";
import { PlusCircle } from "lucide-react";

interface CreateTournamentDialogProps {
  onCreateTournament: (tournament: editTournament) => void;
}

export function CreateTournamentDialog({
  onCreateTournament,
}: CreateTournamentDialogProps) {
  const [open, setOpen] = useState(false);
  const [name, setName] = useState("");
  const [startDate, setStartDate] = useState<string>("");

  const handleCreateTournament = () => {
    if (!name || !startDate) {
      toast({
        title: "Error",
        description:
          "Please provide both a name and a start date for the tournament.",
        variant: "destructive",
      });
      return;
    }

    const newTournament: editTournament = {
      name,
      startDate: new Date(startDate).toISOString(),
    };

    onCreateTournament(newTournament);
    setOpen(false);
    setName("");
    setStartDate("");
    toast({
      title: "Success",
      description: "New tournament created successfully.",
    });
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button data-cy="create-tournament-button">
          <PlusCircle className="mr-2 h-4 w-4" />
          New Tournament
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Create New Tournament</DialogTitle>
          <DialogDescription>
            Enter the details for the new tournament. Click create when
            you&apos;re done.
          </DialogDescription>
        </DialogHeader>
        <div className="grid gap-4 py-4">
          <div className="grid grid-cols-4 items-center gap-4">
            <Label htmlFor="name" className="text-right">
              Name
            </Label>
            <Input
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="col-span-3"
            />
          </div>
          <div className="grid grid-cols-4 items-center gap-4">
            <Label htmlFor="startDate" className="text-right">
              Start Date
            </Label>
            <Input
              type="date"
              id="startDate"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              className="col-span-3"
            />
          </div>
        </div>
        <DialogFooter>
          <Button type="submit" onClick={handleCreateTournament}>
            Create Tournament
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
