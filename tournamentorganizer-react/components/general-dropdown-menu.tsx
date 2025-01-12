import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import Link from "next/link";

interface Action {
  label: string;
  icon: React.ReactNode;
  onClick: (e: React.MouseEvent) => void;
  href?: string;
  "data-cy"?: string;
}

interface GeneralContextMenuProps {
  actions: Action[];
  children: React.ReactNode;
  "data-cy"?: string;
}

export function GeneralDropdownMenu({
  actions,
  children,
  "data-cy": dataCy,
}: GeneralContextMenuProps) {
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild data-cy={`${dataCy}-trigger`}>
        {children}
      </DropdownMenuTrigger>
      <DropdownMenuContent className="w-48" data-cy={`${dataCy}-content`}>
        {actions.map((action, index) => (
          <DropdownMenuItem
            key={index}
            onClick={action.onClick}
            data-cy={action["data-cy"] || `${dataCy}-action-${index}`}
          >
            {action.href ? (
              <Link
                href={action.href}
                className="flex items-center w-full"
                onClick={(e) => e.stopPropagation()}
                data-cy={`${dataCy}-link-${index}`}
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
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
