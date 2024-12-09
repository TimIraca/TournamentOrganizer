"use client";

import { ModeToggle } from "@/components/modetoggle";
import { SidebarTrigger } from "@/components/ui/sidebar";

export default function Page() {
  return (
    <div>
      <div className="flex items-center space-x-4">
        <SidebarTrigger />
        <ModeToggle />
      </div>
    </div>
  );
}
