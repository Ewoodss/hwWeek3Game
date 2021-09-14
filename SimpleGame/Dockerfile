#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
EXPOSE 42069
WORKDIR /src
COPY ["SimpleGame/SimpleGame.csproj", "SimpleGame/"]
RUN dotnet restore "SimpleGame/SimpleGame.csproj"
COPY . .
WORKDIR "/src/SimpleGame"
RUN dotnet build "SimpleGame.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleGame.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleGame.dll"]