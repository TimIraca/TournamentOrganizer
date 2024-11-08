FROM node:18-alpine

WORKDIR /app

# Copy package files
COPY package.json package-lock.json* ./

# Install dependencies
RUN npm ci

# Copy the rest of the application
COPY . .

# Build the application
RUN npm run build

# Expose port
EXPOSE 3000

# Set host to allow connections from outside
ENV HOSTNAME "0.0.0.0"
ENV PORT 3000
ENV NODE_ENV production

# Start the application
CMD ["npm", "start"]