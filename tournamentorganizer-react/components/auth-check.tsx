"use client";

import { useEffect } from "react";
import { useRouter, usePathname } from "next/navigation";

export function AuthCheck({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    const token = localStorage.getItem("token");
    const isAuthPage = pathname.startsWith("/auth");

    if (!token && !isAuthPage) {
      router.push("/auth/login");
    }

    if (token && isAuthPage) {
      router.push("/tournaments");
    }
  }, [pathname, router]);

  return <>{children}</>;
}
