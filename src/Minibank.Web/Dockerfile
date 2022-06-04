#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Minibank.Web/Minibank.Web.csproj", "Minibank.Web/"]
COPY ["Minibank.Core/Minibank.Core.csproj", "Minibank.Core/"]
COPY ["Minibank.Data/Minibank.Data.csproj", "Minibank.Data/"]
RUN dotnet restore "Minibank.Web/Minibank.Web.csproj"
COPY . .
WORKDIR "/src/Minibank.Web"
RUN dotnet build "Minibank.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Minibank.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Minibank.Web.dll"]