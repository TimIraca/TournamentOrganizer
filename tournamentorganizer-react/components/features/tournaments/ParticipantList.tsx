import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Participant } from "@/types";
import { formatDate } from "@/lib/utils";

interface ParticipantListProps {
  participants: Participant[];
  maxParticipants: number;
  onRemoveParticipant?: (participantId: string) => Promise<void>;
}

export function ParticipantList({
  participants,
  maxParticipants,
  onRemoveParticipant,
}: ParticipantListProps) {
  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h3 className="text-lg font-medium">
          Participants ({participants.length}/{maxParticipants})
        </h3>
      </div>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Name</TableHead>
            <TableHead>Registration Date</TableHead>
            <TableHead></TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {participants.map((participant) => (
            <TableRow key={participant.id}>
              <TableCell>{participant.participantName}</TableCell>
              <TableCell>{formatDate(participant.registrationDate)}</TableCell>
              <TableCell className="text-right">
                {onRemoveParticipant && (
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => onRemoveParticipant(participant.id)}
                  >
                    Remove
                  </Button>
                )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
