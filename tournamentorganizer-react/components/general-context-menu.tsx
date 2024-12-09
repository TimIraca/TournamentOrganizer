import React from "react";
import {
  ContextMenu,
  ContextMenuContent,
  ContextMenuItem,
  ContextMenuTrigger,
} from "@/components/ui/context-menu";
import Link from "next/link";

interface Action {
  label: string;
  icon: React.ReactNode;
  onClick: (e: React.MouseEvent) => void;
  href?: string;
}

interface GeneralContextMenuProps {
  actions: Action[];
  children: React.ReactNode;
}

export function GeneralContextMenu({
  actions,
  children,
}: GeneralContextMenuProps) {
  return (
    <ContextMenu>
      <ContextMenuTrigger asChild>{children}</ContextMenuTrigger>
      <ContextMenuContent className="w-48">
        {actions.map((action, index) => (
          <ContextMenuItem key={index} onClick={action.onClick}>
            {action.href ? (
              <Link
                href={action.href}
                className="flex items-center w-full"
                onClick={(e) => e.stopPropagation()}
              >
                {action.icon}
                <span className="ml-2">{action.label}</span>
              </Link>
            ) : (
              <>
                {action.icon}
                <span className="ml-2">{action.label}</span>
              </>
            )}
          </ContextMenuItem>
        ))}
      </ContextMenuContent>
    </ContextMenu>
  );
}
