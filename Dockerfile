# From base image for build (with sdk)
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS src
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src
# Copy src to image
COPY . .

# Run building the project with packets resoring
RUN dotnet build src/Minibank.Web -c Release -r linux-x64
# Run tests
RUN dotnet test src/Minibank.Core.Tests --no-build
# Publish projects' dll to /dist directory
RUN dotnet publish src/Minibank.Web -c Release -r linux-x64 --no-build -o /dist

# From base image with runtime only
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as final
WORKDIR /app
# Copy from src image to final image
COPY --from=src /dist .
# Declare environment variables
ENV ASPNETCORE_URLS=http://localhost:5001;http://localhost:5000;
EXPOSE 5000 5001
# Start the application
ENTRYPOINT ["dotnet", "Minibank.Web.dll"]