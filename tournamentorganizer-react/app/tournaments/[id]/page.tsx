"use client";

import { ModeToggle } from "@/components/modetoggle";
import { SidebarTrigger } from "@/components/ui/sidebar";
import { Button } from "@/components/ui/button";
import { Calendar, CheckCircle, Clock, Edit } from "lucide-react";
import { useParams } from "next/navigation";
import Link from "next/link";
import { TournamentBracket } from "./tournament-bracket";
import { Badge } from "@/components/ui/badge";
import { tournamentApi } from "@/lib/api";
import { useEffect, useState } from "react";

export default function Page() {
  const params = useParams();
  const id = params?.id;
  const [tournamentName, setTournamentName] = useState<string | null>(null);

  useEffect(() => {
    const fetchTournamentName = async () => {
      try {
        const tournamentDetails = await tournamentApi.getTournament(
          id as string
        );
        setTournamentName(tournamentDetails.name);
      } catch (error) {
        console.error("Error fetching tournament details:", error);
      }
    };

    if (id) {
      fetchTournamentName();
    }
  }, [id]);
  return (
    <div className="flex flex-col gap-6 p-6 w-full">
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-4">
          <SidebarTrigger />
          <ModeToggle />
        </div>
        <h1 className="text-3xl font-bold tracking-tight text-center flex-grow">
          Tournament: {tournamentName || "Loading..."}
        </h1>
        <Link href={`/tournaments/${id}/edit/`}>
          <Button>
            <Edit className="mr-2 h-4 w-4" />
            Edit Tournament
          </Button>
        </Link>
      </div>
      <div className="container mx-auto">
        <div className="flex space-x-4 mb-4">
          <Badge variant="secondary" className="bg-green-100 text-green-800">
            <CheckCircle className="w-4 h-4 mr-1" />
            Completed
          </Badge>
          <Badge variant="secondary" className="bg-yellow-100 text-yellow-800">
            <Clock className="w-4 h-4 mr-1" />
            In Progress
          </Badge>
          <Badge variant="secondary" className="bg-blue-100 text-blue-800">
            <Calendar className="w-4 h-4 mr-1" />
            Upcoming
          </Badge>
        </div>
        <TournamentBracket tournamentId={id as string} />
      </div>
    </div>
  );
}
