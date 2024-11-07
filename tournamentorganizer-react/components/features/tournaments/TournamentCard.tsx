import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  CardDescription,
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Tournament } from "@/types";
import { formatDate } from "@/lib/utils"; // We'll create this utility

interface TournamentCardProps {
  tournament: Tournament;
  onClick: (tournament: Tournament) => void;
}

export function TournamentCard({ tournament, onClick }: TournamentCardProps) {
  return (
    <Card
      className="cursor-pointer hover:bg-accent/50"
      onClick={() => onClick(tournament)}
    >
      <CardHeader>
        <CardTitle>{tournament.name}</CardTitle>
        <CardDescription>{formatDate(tournament.startDate)}</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="flex flex-col gap-2">
          <div className="flex items-center justify-between">
            <span className="text-sm text-muted-foreground">
              {tournament.currentParticipants}/{tournament.maxParticipants}{" "}
              Participants
            </span>
            <Badge
              variant={
                tournament.status === "Registration" ? "default" : "secondary"
              }
            >
              {tournament.status}
            </Badge>
          </div>
          <div className="flex items-center justify-between">
            <span className="text-sm text-muted-foreground">
              Prize Pool: {tournament.prizePool} {tournament.prizeCurrency}
            </span>
            <span className="text-sm font-medium">{tournament.format}</span>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
