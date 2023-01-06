FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/PcStatsReporterBackend.AspNet/PcStatsReporterBackend.AspNet.csproj", "PcStatsReporterBackend.AspNet/"]
RUN dotnet restore "src/PcStatsReporterBackend.AspNet/PcStatsReporterBackend.AspNet.csproj"
COPY . .
WORKDIR "/src/PcStatsReporterBackend.AspNet"
RUN dotnet build "PcStatsReporterBackend.AspNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PcStatsReporterBackend.AspNet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PcStatsReporterBackend.AspNet.dll"]
