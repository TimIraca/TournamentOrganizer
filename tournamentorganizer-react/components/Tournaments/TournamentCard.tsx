import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  CardDescription,
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import type { Tournament } from "@/types";
import { formatDate } from "@/lib/utils";

interface TournamentCardProps {
  tournament: Tournament;
}

export function TournamentCard({ tournament }: TournamentCardProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>{tournament.name}</CardTitle>
        <CardDescription>{formatDate(tournament.startDate)}</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="flex items-center justify-between">
          <span className="text-sm text-muted-foreground">
            Tournament ID: {tournament.id}
          </span>
          <Badge variant={tournament.isCompleted ? "secondary" : "default"}>
            {tournament.isCompleted ? "Completed" : "Ongoing"}
          </Badge>
        </div>
      </CardContent>
    </Card>
  );
}
