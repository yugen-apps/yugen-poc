# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Poc.Redis.Subscriber/Poc.Redis.Subscriber.csproj", "Poc.Redis.Subscriber/"]
COPY ["Poc.Redis.Common/Poc.Redis.Common.csproj", "Poc.Redis.Common/"]
RUN dotnet restore "Poc.Redis.Subscriber/Poc.Redis.Subscriber.csproj"
COPY . .
WORKDIR "/src/Poc.Redis.Subscriber"
RUN dotnet build "Poc.Redis.Subscriber.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Poc.Redis.Subscriber.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poc.Redis.Subscriber.dll"]