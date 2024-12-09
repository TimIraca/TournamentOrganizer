import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  eslint: {
    dirs: ["app"],
  },
  async rewrites() {
    return [
      {
        source: "/api/:path*",
        destination: "http://tournamentorganizer.api:80/api/:path*",
      },
    ];
  },
};

export default nextConfig;
