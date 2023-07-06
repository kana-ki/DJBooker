FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DJBooker/DJBooker.csproj", "DJBooker/"]
RUN dotnet restore "DJBooker/DJBooker.csproj"
COPY . .
WORKDIR "/src/DJBooker"
RUN dotnet build "DJBooker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DJBooker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DJBooker.dll"]
