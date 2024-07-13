FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src
COPY ["Services/Product.API/Product.API.csproj","Services/Product.API/"]
COPY ["BuildingBlock/Common.Logging/Common.logging.csproj","BuildingBlock/Common.Logging/"]
COPY ["BuildingBlock/Contracts/Contracts.csproj","BuildingBlock/Contracts/"]
COPY ["BuildingBlock/Infrastructure/Infrastructure.csproj","BuildingBlock/Infrastructure/"]
COPY ["BuildingBlock/Shared/Shared.csproj","BuildingBlock/Shared/"]

RUN dotnet restore "Services/Product.API/Product.API.csproj"
COPY . .

WORKDIR "src/Services/Product.API"
RUN dotnet build "Product.API.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish  "Product.API.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.API.dll"]