// components/features/tournaments/TournamentDetails.tsx
import { useState } from "react";
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { handleApiError } from "@/lib/api";
import type { Tournament, TournamentStatus } from "@/types";
import { formatDate } from "@/lib/utils";

interface TournamentDetailsProps {
  tournament: Tournament;
  onStartTournament?: () => Promise<void>;
}

export function TournamentDetails({
  tournament,
  onStartTournament,
}: TournamentDetailsProps) {
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({
    name: tournament.name,
    description: tournament.description,
    format: tournament.format,
    startDate: tournament.startDate,
    maxParticipants: tournament.maxParticipants,
    prizePool: tournament.prizePool,
    prizeCurrency: tournament.prizeCurrency,
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      // Assuming you'll add an updateTournament endpoint to your API client
      // await tournamentApi.updateTournament(tournament.id, formData);
      setIsEditing(false);
    } catch (error) {
      console.error("Failed to update tournament:", handleApiError(error));
    }
  };

  const formatStatusBadge = (
    status: TournamentStatus
  ): "default" | "secondary" | "outline" | "destructive" | null | undefined => {
    const variants: Record<
      TournamentStatus,
      "default" | "secondary" | "outline"
    > = {
      Registration: "default",
      InProgress: "secondary",
      Completed: "outline",
    };
    return variants[status];
  };

  if (!isEditing) {
    return (
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <div className="space-y-1">
            <CardTitle>{tournament.name}</CardTitle>
            <CardDescription>
              {formatDate(tournament.startDate)}
            </CardDescription>
          </div>
          <Badge variant={formatStatusBadge(tournament.status)}>
            {tournament.status}
          </Badge>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <div className="grid gap-1">
              <label className="text-sm font-medium">Description</label>
              <p className="text-sm text-muted-foreground">
                {tournament.description}
              </p>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="grid gap-1">
                <label className="text-sm font-medium">Format</label>
                <p className="text-sm text-muted-foreground">
                  {tournament.format}
                </p>
              </div>
              <div className="grid gap-1">
                <label className="text-sm font-medium">Participants</label>
                <p className="text-sm text-muted-foreground">
                  {tournament.currentParticipants} /{" "}
                  {tournament.maxParticipants}
                </p>
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="grid gap-1">
                <label className="text-sm font-medium">Prize Pool</label>
                <p className="text-sm text-muted-foreground">
                  {tournament.prizePool} {tournament.prizeCurrency}
                </p>
              </div>
            </div>

            <div className="flex gap-2">
              {tournament.status === "Registration" && onStartTournament && (
                <Button onClick={onStartTournament} className="flex-1">
                  Start Tournament
                </Button>
              )}
              <Button
                variant="outline"
                onClick={() => setIsEditing(true)}
                className="flex-1"
              >
                Edit Tournament
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Edit Tournament</CardTitle>
        <CardDescription>Update tournament details</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid gap-2">
            <label className="text-sm font-medium">Tournament Name</label>
            <Input
              value={formData.name}
              onChange={(e) =>
                setFormData((prev) => ({ ...prev, name: e.target.value }))
              }
            />
          </div>

          <div className="grid gap-2">
            <label className="text-sm font-medium">Description</label>
            <Input
              value={formData.description}
              onChange={(e) =>
                setFormData((prev) => ({
                  ...prev,
                  description: e.target.value,
                }))
              }
            />
          </div>

          {/* <div className="grid gap-2">
            <label className="text-sm font-medium">Format</label>
            <Select
              value={formData.format}
              onValueChange={(value: TournamentFormat) => 
                setFormData(prev => ({ ...prev, format: value }))
              }
            >
              <SelectTrigger>
                <SelectValue placeholder="Select format" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="SingleElimination">Single Elimination</SelectItem>
                <SelectItem value="RoundRobin">Round Robin</SelectItem>
              </SelectContent>
            </Select>
          </div> */}

          <div className="grid gap-2">
            <label className="text-sm font-medium">Start Date</label>
            <Input
              type="datetime-local"
              value={formData.startDate.split(".")[0]} // Remove milliseconds if present
              onChange={(e) =>
                setFormData((prev) => ({ ...prev, startDate: e.target.value }))
              }
            />
          </div>

          <div className="grid gap-2">
            <label className="text-sm font-medium">Maximum Participants</label>
            <Input
              type="number"
              value={formData.maxParticipants}
              onChange={(e) =>
                setFormData((prev) => ({
                  ...prev,
                  maxParticipants: parseInt(e.target.value, 10),
                }))
              }
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="grid gap-2">
              <label className="text-sm font-medium">Prize Pool</label>
              <Input
                type="number"
                value={formData.prizePool}
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    prizePool: parseInt(e.target.value, 10),
                  }))
                }
              />
            </div>
            <div className="grid gap-2">
              <label className="text-sm font-medium">Currency</label>
              <Input
                value={formData.prizeCurrency}
                onChange={(e) =>
                  setFormData((prev) => ({
                    ...prev,
                    prizeCurrency: e.target.value,
                  }))
                }
              />
            </div>
          </div>

          <div className="flex gap-2">
            <Button type="submit" className="flex-1">
              Save Changes
            </Button>
            <Button
              type="button"
              variant="outline"
              onClick={() => setIsEditing(false)}
              className="flex-1"
            >
              Cancel
            </Button>
          </div>
        </form>
      </CardContent>
    </Card>
  );
}
