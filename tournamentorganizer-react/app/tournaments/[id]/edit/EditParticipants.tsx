"use client";

import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Trash2, Plus, Loader2 } from "lucide-react";
import { toast } from "@/hooks/use-toast";
import { Participant } from "@/types";
import { tournamentApi } from "@/lib/api";
import { useParams } from "next/navigation";

export default function EditParticipants() {
  const params = useParams();
  const tournamentId = params?.id;
  const [participants, setParticipants] = useState<Participant[]>([]);
  const [newParticipantName, setNewParticipantName] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadParticipants = async () => {
      try {
        const data = await tournamentApi.getParticipants(
          tournamentId as string
        );
        setParticipants(data);
        setLoading(false);
      } catch (err) {
        setError(`Failed to load participants ${err}`);
        setLoading(false);
      }
    };

    loadParticipants();
  }, [tournamentId]);

  const handleAddParticipant = async () => {
    if (!newParticipantName.trim()) return;

    try {
      const newParticipant = await tournamentApi.addParticipant(
        tournamentId as string,
        newParticipantName
      );
      setParticipants([...participants, newParticipant]);
      setNewParticipantName("");
      toast({
        title: "Participant added",
        description: `${newParticipant.name} has been added to the tournament.`,
      });
    } catch {
      toast({
        title: "Error:",
        description: "Failed to add participant. Please try again.",
        variant: "destructive",
      });
    }
  };

  const handleUpdateParticipant = async (id: string, newName: string) => {
    try {
      await tournamentApi.updateParticipant(
        tournamentId as string,
        id,
        newName
      );
      setParticipants(
        participants.map((p) => (p.id === id ? { ...p, name: newName } : p))
      );
      toast({
        title: "Participant updated",
        description: `The participant's name has been updated to ${newName}.`,
      });
    } catch {
      toast({
        title: "Error",
        description: "Failed to update participant. Please try again.",
        variant: "destructive",
      });
    }
  };

  const handleDeleteParticipant = async (id: string) => {
    try {
      await tournamentApi.deleteParticipant(tournamentId as string, id);
      setParticipants(participants.filter((p) => p.id !== id));
      toast({
        title: "Participant deleted",
        description: "The participant has been removed from the tournament.",
      });
    } catch {
      toast({
        title: "Error",
        description: "Failed to delete participant. Please try again.",
        variant: "destructive",
      });
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Loader2 className="h-8 w-8 animate-spin" />
      </div>
    );
  }

  if (error) {
    return <div className="text-center text-red-500">{error}</div>;
  }

  return (
    <Card className="w-full max-w-2xl mx-auto" data-cy="edit-participants-card">
      <CardHeader>
        <CardTitle>
          Edit Tournament Participants ({participants.length})
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          <div className="flex space-x-2">
            <Input
              data-cy="new-participant-input"
              placeholder="New participant name"
              value={newParticipantName}
              onChange={(e) => setNewParticipantName(e.target.value)}
            />
            <Button data-cy="add-participant" onClick={handleAddParticipant}>
              <Plus className="h-4 w-4 mr-2" /> Add
            </Button>
          </div>

          {participants.map((participant) => (
            <div
              key={participant.id}
              className="flex items-center space-x-2"
              data-cy="participant-row"
              data-participant-id={participant.id}
            >
              <Input
                data-cy="participant-name-input"
                value={participant.name}
                onChange={(e) =>
                  handleUpdateParticipant(participant.id, e.target.value)
                }
              />
              <Button
                data-cy="delete-participant-button"
                variant="destructive"
                size="icon"
                onClick={() => handleDeleteParticipant(participant.id)}
              >
                <Trash2 className="h-4 w-4" />
              </Button>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
