FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CSharpChatSystem.Client/CSharpChatSystem.Client.csproj", "CSharpChatSystem.Client/"]
RUN dotnet restore "CSharpChatSystem.Client/CSharpChatSystem.Client.csproj"
COPY . .
WORKDIR "/src/CSharpChatSystem.Client"
RUN dotnet build "CSharpChatSystem.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSharpChatSystem.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSharpChatSystem.Client.dll"]
