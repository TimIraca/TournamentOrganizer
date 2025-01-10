"use client";
import * as React from "react";
import { useEffect, useState } from "react";
import {
  AudioWaveform,
  BookOpen,
  Bot,
  Command,
  Frame,
  GalleryVerticalEnd,
  Map,
  PieChart,
  Settings2,
  SquareTerminal,
  Trophy,
} from "lucide-react";
import { NavMain } from "@/components/nav-main";
import { NavProjects } from "@/components/nav-projects";
import { NavUser } from "@/components/nav-user";
import { TeamSwitcher } from "@/components/team-switcher";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarRail,
} from "@/components/ui/sidebar";
import { ModeToggle } from "./modetoggle";
import { jwtDecode } from "jwt-decode";

interface DecodedToken {
  username?: string;
}

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  const [username, setUsername] = useState("notfound");

  useEffect(() => {
    if (typeof window !== "undefined") {
      const token = localStorage.getItem("token");
      if (token) {
        try {
          const decodedToken = jwtDecode<DecodedToken>(token);
          setUsername(decodedToken?.username || "notfound");
        } catch (error) {
          console.error("Failed to decode token", error);
        }
      }
    }
  }, []);

  // This is sample data that uses the username state
  const data = {
    user: {
      name: username,
      email: "example@example.com",
      avatar: "/avatars/shadcn.jpg",
    },
    teams: [
      {
        name: "Tournament Organizer",
        logo: Trophy,
        plan: "organizers",
      },
    ],
    navMain: [
      {
        title: "Tournaments",
        url: "/tournaments",
        icon: Trophy,
        isActive: true,
        items: [
          {
            title: "Upcoming",
            url: "tournaments/upcoming",
          },
          {
            title: "Past",
            url: "tournaments/past",
          },
        ],
      },
    ],
    projects: [
      {
        name: "Design Engineering",
        url: "#",
        icon: Frame,
      },
      {
        name: "Sales & Marketing",
        url: "#",
        icon: PieChart,
      },
      {
        name: "Travel",
        url: "#",
        icon: Map,
      },
    ],
  };

  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader>
        <TeamSwitcher teams={data.teams} />
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={data.navMain} />
        {/* <NavProjects projects={data.projects} /> */}
        {/* <ModeToggle></ModeToggle> */}
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={data.user} />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  );
}
