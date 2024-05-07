#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroServices/gRPC/gRPC.client/gRPC.client.csproj", "MicroServices/gRPC/gRPC.client/"]
COPY ["MicroServices/gRPC/gRPC.server/gRPC.server.csproj", "MicroServices/gRPC/gRPC.server/"]
COPY ["MicroServices/gRPC/gRPC.DataAccess/gRPC.DataAccess.csproj", "MicroServices/gRPC/gRPC.DataAccess/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "./MicroServices/gRPC/gRPC.client/./gRPC.client.csproj"
COPY . .
WORKDIR "/src/MicroServices/gRPC/gRPC.client"
RUN dotnet build "./gRPC.client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./gRPC.client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gRPC.client.dll"]