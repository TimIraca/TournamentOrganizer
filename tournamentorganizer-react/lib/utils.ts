import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleDateString("en-US", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
}

export const formatDateInput = (input: string): string => {
  const cleaned = input.replace(/\D/g, "");

  if (cleaned.length >= 4) {
    const day = cleaned.slice(0, 2);
    const month = cleaned.slice(2, 4);
    const year = cleaned.slice(4, 8);

    let formatted = `${day}/${month}`;
    if (year) {
      formatted += `/${year}`;
    }
    return formatted;
  } else if (cleaned.length >= 2) {
    return `${cleaned.slice(0, 2)}/${cleaned.slice(2)}`;
  }
  return cleaned;
};

export const validateDate = (dateStr: string): boolean => {
  const pattern = /^(\d{2})\/(\d{2})\/(\d{4})$/;
  const match = dateStr.match(pattern);

  if (!match) return false;

  const day = parseInt(match[1], 10);
  const month = parseInt(match[2], 10);
  const year = parseInt(match[3], 10);

  if (month < 1 || month > 12) return false;
  if (day < 1 || day > 31) return false;
  if (year < 1900 || year > 2100) return false;

  const daysInMonth = new Date(year, month, 0).getDate();
  if (day > daysInMonth) return false;

  return true;
};

export const convertToISODate = (dateStr: string): string => {
  const [day, month, year] = dateStr.split("/");
  const isoDate = `${year}-${month}-${day}`;
  return new Date(isoDate).toISOString();
};
