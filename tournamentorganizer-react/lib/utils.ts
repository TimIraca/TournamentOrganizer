import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"
import type { TournamentStatus } from "@/types"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}
export function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
}

export function getTournamentStatusColor(status: TournamentStatus): string {
  switch (status) {
    case 'Registration':
      return 'bg-green-500';
    case 'InProgress':
      return 'bg-blue-500';
    case 'Completed':
      return 'bg-gray-500';
    default:
      return 'bg-gray-500';
  }
}