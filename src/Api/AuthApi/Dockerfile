FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuthApi/AuthApi.csproj", "AuthApi/"]
RUN dotnet restore "AuthApi/AuthApi.csproj"
COPY . .
WORKDIR "/src/AuthApi"
RUN dotnet build "AuthApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthApi.dll"]
