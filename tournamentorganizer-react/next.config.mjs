/** @type {import('next').NextConfig} */
const nextConfig = {
  output: 'standalone',
  experimental: {
      outputFileTracingRoot: undefined,
  },
  async rewrites() {
      return [
          {
              source: '/api/:path*',
              destination: 'http://tournamentorganizer.api:80/api/:path*'  // Use HTTP and internal Docker network
          }
      ];
  }
};

export default nextConfig;