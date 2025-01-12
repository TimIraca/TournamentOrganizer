"use client";
import { Suspense } from "react";
import { useParams } from "next/navigation";
import { Skeleton } from "@/components/ui/skeleton";
import EditParticipants from "./EditParticipants";
import EditTournament from "./EditTournament";
import { ModeToggle } from "@/components/modetoggle";
import { SidebarTrigger } from "@/components/ui/sidebar";
import { ArrowLeft } from "lucide-react";
import Link from "next/link";
import { Button } from "@/components/ui/button";

export default function EditTournamentPage() {
  const params = useParams();
  const id = params?.id;

  return (
    <div className="flex flex-col gap-6 p-6 w-full">
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-4">
          <SidebarTrigger />
          <ModeToggle />
        </div>
        <h1 className="text-3xl font-bold tracking-tight text-center flex-grow">
          Edit Tournament
        </h1>
        <Link href={`/tournaments/${id}/`}>
          <Button data-cy="return-to-tournament-button">
            <ArrowLeft className="h-4 w-4" />
            Return to Tournament
          </Button>
        </Link>
      </div>
      <div className="container mx-auto py-10">
        <Suspense fallback={<Skeleton className="w-full h-[600px]" />}>
          <EditTournament />
          <EditParticipants />
        </Suspense>
      </div>
    </div>
  );
}
