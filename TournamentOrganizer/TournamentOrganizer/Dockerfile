#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files first and restore dependencies
COPY ["../../TournamentOrganizer/TournamentOrganizer.api.csproj", "TournamentOrganizer/"]
COPY ["../../TournamentOrganizer.Core/TournamentOrganizer.Core.csproj", "TournamentOrganizer.Core/"]
COPY ["../../TournamentOrganizer.DAL/TournamentOrganizer.DAL.csproj", "TournamentOrganizer.DAL/"]

# Restore packages
RUN dotnet restore "TournamentOrganizer/TournamentOrganizer.api.csproj"

# Copy the rest of the source code
COPY ../.. .

# Build the application
WORKDIR "/src/TournamentOrganizer"
RUN dotnet build "TournamentOrganizer.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TournamentOrganizer.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TournamentOrganizer.api.dll"]