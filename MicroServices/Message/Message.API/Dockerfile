#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroServices/Message/Message.API/Message.API.csproj", "MicroServices/Message/Message.API/"]
COPY ["MicroServices/Message/Message.DataAccess/Message.DataAccess.csproj", "MicroServices/Message/Message.DataAccess/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "./MicroServices/Message/Message.API/./Message.API.csproj"
COPY . .
WORKDIR "/src/MicroServices/Message/Message.API"
RUN dotnet build "./Message.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Message.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Message.API.dll"]