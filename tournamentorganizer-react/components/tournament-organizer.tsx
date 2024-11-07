"use client";

import { useState } from "react";
import {
  Trophy,
  Users,
  Calendar,
  Settings,
  BarChart,
  HelpCircle,
  MessageSquare,
  ChevronRight,
  MoreHorizontal,
  PlusCircle,
} from "lucide-react";

import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import { Separator } from "@/components/ui/separator";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarInset,
  SidebarMenu,
  SidebarMenuAction,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarMenuSub,
  SidebarMenuSubButton,
  SidebarMenuSubItem,
  SidebarProvider,
  SidebarTrigger,
} from "@/components/ui/sidebar";

const data = {
  user: {
    name: "Sarah Johnson",
    email: "sarah@tournamentorganizer.com",
    avatar: "/placeholder.svg?height=32&width=32",
  },
  navMain: [
    {
      title: "Tournaments",
      url: "#",
      icon: Trophy,
      isActive: true,
      items: [
        { title: "Active", url: "#" },
        { title: "Upcoming", url: "#" },
        { title: "Past", url: "#" },
      ],
    },
    {
      title: "Teams",
      url: "#",
      icon: Users,
      items: [
        { title: "Manage", url: "#" },
        { title: "Registrations", url: "#" },
      ],
    },
    {
      title: "Schedule",
      url: "#",
      icon: Calendar,
      items: [
        { title: "Matches", url: "#" },
        { title: "Venues", url: "#" },
      ],
    },
    {
      title: "Analytics",
      url: "#",
      icon: BarChart,
      items: [
        { title: "Performance", url: "#" },
        { title: "Reports", url: "#" },
      ],
    },
    {
      title: "Settings",
      url: "#",
      icon: Settings,
      items: [
        { title: "General", url: "#" },
        { title: "Notifications", url: "#" },
        { title: "Security", url: "#" },
      ],
    },
  ],
  navSecondary: [
    { title: "Help Center", url: "#", icon: HelpCircle },
    { title: "Feedback", url: "#", icon: MessageSquare },
  ],
};

const tournaments = [
  {
    id: 1,
    name: "Summer Classic",
    date: "Aug 15-20, 2024",
    teams: 16,
    status: "Upcoming",
  },
  {
    id: 2,
    name: "Fall Cup",
    date: "Oct 5-10, 2024",
    teams: 32,
    status: "Registration Open",
  },
  {
    id: 3,
    name: "Winter Challenge",
    date: "Jan 7-12, 2025",
    teams: 24,
    status: "Planning",
  },
];

export function TournamentOrganizer() {
  const [selectedTournament, setSelectedTournament] = useState(tournaments[0]);

  return (
    <SidebarProvider>
      <Sidebar variant="inset">
        <SidebarHeader>
          <SidebarMenu>
            <SidebarMenuItem>
              <SidebarMenuButton size="lg" asChild>
                <a href="#">
                  <div className="flex aspect-square size-8 items-center justify-center rounded-lg bg-primary text-primary-foreground">
                    <Trophy className="size-4" />
                  </div>
                  <div className="grid flex-1 text-left text-sm leading-tight">
                    <span className="truncate font-semibold">
                      TournamentPro
                    </span>
                    <span className="truncate text-xs">
                      Organizer Dashboard
                    </span>
                  </div>
                </a>
              </SidebarMenuButton>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarHeader>
        <SidebarContent>
          <SidebarGroup>
            <SidebarGroupLabel>Management</SidebarGroupLabel>
            <SidebarMenu>
              {data.navMain.map((item) => (
                <Collapsible
                  key={item.title}
                  asChild
                  defaultOpen={item.isActive}
                >
                  <SidebarMenuItem>
                    <SidebarMenuButton asChild tooltip={item.title}>
                      <a href={item.url}>
                        <item.icon />
                        <span>{item.title}</span>
                      </a>
                    </SidebarMenuButton>
                    {item.items?.length ? (
                      <>
                        <CollapsibleTrigger asChild>
                          <SidebarMenuAction className="data-[state=open]:rotate-90">
                            <ChevronRight />
                            <span className="sr-only">Toggle</span>
                          </SidebarMenuAction>
                        </CollapsibleTrigger>
                        <CollapsibleContent>
                          <SidebarMenuSub>
                            {item.items?.map((subItem) => (
                              <SidebarMenuSubItem key={subItem.title}>
                                <SidebarMenuSubButton asChild>
                                  <a href={subItem.url}>
                                    <span>{subItem.title}</span>
                                  </a>
                                </SidebarMenuSubButton>
                              </SidebarMenuSubItem>
                            ))}
                          </SidebarMenuSub>
                        </CollapsibleContent>
                      </>
                    ) : null}
                  </SidebarMenuItem>
                </Collapsible>
              ))}
            </SidebarMenu>
          </SidebarGroup>
          <SidebarGroup className="mt-auto">
            <SidebarGroupContent>
              <SidebarMenu>
                {data.navSecondary.map((item) => (
                  <SidebarMenuItem key={item.title}>
                    <SidebarMenuButton asChild size="sm">
                      <a href={item.url}>
                        <item.icon />
                        <span>{item.title}</span>
                      </a>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                ))}
              </SidebarMenu>
            </SidebarGroupContent>
          </SidebarGroup>
        </SidebarContent>
        <SidebarFooter>
          <SidebarMenu>
            <SidebarMenuItem>
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <SidebarMenuButton
                    size="lg"
                    className="data-[state=open]:bg-accent data-[state=open]:text-accent-foreground"
                  >
                    <Avatar className="h-8 w-8">
                      <AvatarImage
                        src={data.user.avatar}
                        alt={data.user.name}
                      />
                      <AvatarFallback>SJ</AvatarFallback>
                    </Avatar>
                    <div className="grid flex-1 text-left text-sm leading-tight">
                      <span className="truncate font-semibold">
                        {data.user.name}
                      </span>
                      <span className="truncate text-xs">
                        {data.user.email}
                      </span>
                    </div>
                    <MoreHorizontal className="ml-auto size-4" />
                  </SidebarMenuButton>
                </DropdownMenuTrigger>
                <DropdownMenuContent className="w-56" align="end" forceMount>
                  <DropdownMenuLabel className="font-normal">
                    <div className="flex flex-col space-y-1">
                      <p className="text-sm font-medium leading-none">
                        {data.user.name}
                      </p>
                      <p className="text-xs leading-none text-muted-foreground">
                        {data.user.email}
                      </p>
                    </div>
                  </DropdownMenuLabel>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem>Profile</DropdownMenuItem>
                  <DropdownMenuItem>Billing</DropdownMenuItem>
                  <DropdownMenuItem>Settings</DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem>Log out</DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarFooter>
      </Sidebar>
      <SidebarInset>
        <header className="flex h-16 shrink-0 items-center gap-2 border-b px-6">
          <SidebarTrigger />
          <Separator orientation="vertical" className="h-6" />
          <Breadcrumb>
            <BreadcrumbList>
              <BreadcrumbItem>
                <BreadcrumbLink href="#">Tournaments</BreadcrumbLink>
              </BreadcrumbItem>
              <BreadcrumbSeparator />
              <BreadcrumbItem>
                <BreadcrumbPage>Manage</BreadcrumbPage>
              </BreadcrumbItem>
            </BreadcrumbList>
          </Breadcrumb>
        </header>
        <div className="flex flex-col gap-6 p-6">
          <div className="flex items-center justify-between">
            <h1 className="text-3xl font-bold tracking-tight">Tournaments</h1>
            <Button>
              <PlusCircle className="mr-2 h-4 w-4" />
              New Tournament
            </Button>
          </div>
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {tournaments.map((tournament) => (
              <Card
                key={tournament.id}
                className="cursor-pointer hover:bg-accent/50"
                onClick={() => setSelectedTournament(tournament)}
              >
                <CardHeader>
                  <CardTitle>{tournament.name}</CardTitle>
                  <CardDescription>{tournament.date}</CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">
                      {tournament.teams} Teams
                    </span>
                    <Badge
                      variant={
                        tournament.status === "Upcoming"
                          ? "default"
                          : "secondary"
                      }
                    >
                      {tournament.status}
                    </Badge>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
          {selectedTournament && (
            <Card>
              <CardHeader>
                <CardTitle>
                  Tournament Details: {selectedTournament.name}
                </CardTitle>
                <CardDescription>{selectedTournament.date}</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  <div className="grid gap-2">
                    <label
                      htmlFor="tournament-name"
                      className="text-sm font-medium"
                    >
                      Tournament Name
                    </label>
                    <Input
                      id="tournament-name"
                      defaultValue={selectedTournament.name}
                    />
                  </div>
                  <div className="grid gap-2">
                    <label
                      htmlFor="tournament-date"
                      className="text-sm font-medium"
                    >
                      Date
                    </label>
                    <Input
                      id="tournament-date"
                      defaultValue={selectedTournament.date}
                    />
                  </div>
                  <div className="grid gap-2">
                    <label
                      htmlFor="tournament-teams"
                      className="text-sm font-medium"
                    >
                      Number of Teams
                    </label>
                    <Input
                      id="tournament-teams"
                      type="number"
                      defaultValue={selectedTournament.teams}
                    />
                  </div>
                  <div className="grid gap-2">
                    <label
                      htmlFor="tournament-status"
                      className="text-sm font-medium"
                    >
                      Status
                    </label>
                    <Input
                      id="tournament-status"
                      defaultValue={selectedTournament.status}
                    />
                  </div>
                  <Button className="w-full">Update Tournament</Button>
                </div>
              </CardContent>
            </Card>
          )}
        </div>
      </SidebarInset>
    </SidebarProvider>
  );
}
