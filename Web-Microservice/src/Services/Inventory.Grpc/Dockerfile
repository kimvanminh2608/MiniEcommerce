FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/Inventory.Grpc/Inventory.Grpc.csproj", "Services/Inventory.Grpc/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]

RUN dotnet restore "Services/Inventory.Grpc/Inventory.Grpc.csproj"
COPY . .
WORKDIR "/src/Services/Inventory.Grpc"
RUN dotnet build "Inventory.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Grpc.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Grpc.dll"]