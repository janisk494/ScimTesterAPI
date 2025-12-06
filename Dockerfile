FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working folder inside container
WORKDIR /src

# Copy only csproj first (restore cache optimization)
COPY ScimTester.Api/ScimTester.Api.csproj ScimTester.Api/

# Restore dependencies
RUN dotnet restore ScimTester.Api/ScimTester.Api.csproj

# Copy all code
COPY . .

# Publish the project
RUN dotnet publish ScimTester.Api/ScimTester.Api.csproj -c Release -o /app

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "ScimTester.Api.dll"]
