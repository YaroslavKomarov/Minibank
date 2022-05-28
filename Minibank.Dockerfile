FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY . .

RUN dotnet build src/Minibank.Web -c Release -r linux-x64
RUN dotnet test src/Minibank.Core.Tests --no-build
RUN dotnet publish src/Minibank.Web -c Release -r linux-x64 --no-build -o dist

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=build /app/dist .
ENV ASPNETCORE_URLS=http://localhost:5001;http://localhost:5000
EXPOSE 5000 5001
ENTRYPOINT ["dotnet", "Minibank.Web.dll"]
