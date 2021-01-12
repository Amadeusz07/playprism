FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/TournamentService/Playprism.Services.TournamentService.API/Playprism.Services.TournamentService.API.csproj", "code/Playprism.Services.TournamentService.API/"]
COPY ["Services/TournamentService/Playprism.Services.TournamentService.BLL/Playprism.Services.TournamentService.BLL.csproj", "code/Playprism.Services.TournamentService.BLL/"]
COPY ["Services/TournamentService/Playprism.Services.TournamentService.DAL/Playprism.Services.TournamentService.DAL.csproj", "code/Playprism.Services.TournamentService.DAL/"]
RUN dotnet restore "code/Playprism.Services.TournamentService.API/Playprism.Services.TournamentService.API.csproj"
COPY . .
WORKDIR "src/code/Playprism.Services.TournamentService.API/"
RUN dotnet build "src/code/Playprism.Services.TournamentService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Playprism.Services.TournamentService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Playprism.Services.TournamentService.API.dll"]
