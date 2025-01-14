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
import { formatDateInput, validateDate, convertToISODate } from "@/lib/utils";

interface CreateTournamentDialogProps {
  onCreateTournament: (tournament: editTournament) => void;
}

export function CreateTournamentDialog({
  onCreateTournament,
}: CreateTournamentDialogProps) {
  const [open, setOpen] = useState(false);
  const [name, setName] = useState("");
  const [dateInput, setDateInput] = useState("");

  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const formatted = formatDateInput(e.target.value);
    setDateInput(formatted);
  };

  const handleCreateTournament = () => {
    if (!name || !dateInput || !validateDate(dateInput)) {
      toast({
        title: "Error",
        description:
          "Please provide a valid name and date (DD/MM/YYYY) for the tournament.",
        variant: "destructive",
      });
      return;
    }

    const newTournament: editTournament = {
      name,
      startDate: convertToISODate(dateInput),
    };

    onCreateTournament(newTournament);
    setOpen(false);
    setName("");
    setDateInput("");
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
      <DialogContent className="sm:max-w-md">
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
            <div className="col-span-3">
              <Input
                id="startDate"
                placeholder="DD/MM/YYYY"
                value={dateInput}
                onChange={handleDateChange}
                maxLength={10}
              />
            </div>
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
