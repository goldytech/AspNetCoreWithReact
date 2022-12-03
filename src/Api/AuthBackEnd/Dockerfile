FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuthBackEnd/AuthBackEnd.csproj", "AuthBackEnd/"]
RUN dotnet restore "AuthBackEnd/AuthBackEnd.csproj"
COPY . .
WORKDIR "/src/AuthBackEnd"
RUN dotnet build "AuthBackEnd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthBackEnd.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthBackEnd.dll"]
