# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY src/BlitzPrevair.API/*.csproj ./src/BlitzPrevair.API/
COPY src/BlitzPrevair.Core/*.csproj ./src/BlitzPrevair.Core/
COPY src/BlitzPrevair.Infrastructure/*.csproj ./src/BlitzPrevair.Infrastructure/
COPY src/BlitzPrevair.Shared/*.csproj ./src/BlitzPrevair.Shared/
COPY BlitzPrevair.sln ./

RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish src/BlitzPrevair.API/BlitzPrevair.API.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "BlitzPrevair.API.dll"]
