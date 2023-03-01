FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CSharpChatSystem.Server/CSharpChatSystem.Server.csproj", "CSharpChatSystem.Server/"]
RUN dotnet restore "CSharpChatSystem.Server/CSharpChatSystem.Server.csproj"
COPY . .
WORKDIR "/src/CSharpChatSystem.Server"
RUN dotnet build "CSharpChatSystem.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSharpChatSystem.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSharpChatSystem.Server.dll"]
