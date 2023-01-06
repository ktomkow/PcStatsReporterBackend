FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/PcStatsReporterBackend.Reporter/PcStatsReporterBackend.Reporter.csproj", "PcStatsReporterBackend.Reporter/"]
RUN dotnet restore "src/PcStatsReporterBackend.Reporter/PcStatsReporterBackend.Reporter.csproj"
COPY . .
WORKDIR "/src/PcStatsReporterBackend.Reporter"
RUN dotnet build "PcStatsReporterBackend.Reporter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PcStatsReporterBackend.Reporter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PcStatsReporterBackend.Reporter.dll"]
