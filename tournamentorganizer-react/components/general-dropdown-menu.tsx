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
}
interface GeneralContextMenuProps {
  actions: Action[];
  children: React.ReactNode;
}
export function GeneralDropdownMenu({
  actions,
  children,
}: GeneralContextMenuProps) {
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>{children}</DropdownMenuTrigger>
      <DropdownMenuContent className="w-48">
        {actions.map((action, index) => (
          <DropdownMenuItem key={index} onClick={action.onClick}>
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
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
