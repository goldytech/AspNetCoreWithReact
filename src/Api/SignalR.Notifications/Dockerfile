FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SignalR.Notifications/SignalR.Notifications.csproj", "SignalR.Notifications/"]
RUN dotnet restore "SignalR.Notifications/SignalR.Notifications.csproj"
COPY . .
WORKDIR "/src/SignalR.Notifications"
RUN dotnet build "SignalR.Notifications.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalR.Notifications.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalR.Notifications.dll"]
