# Install python

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get -y update && apt-get install -y python3
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["onlineIdeSample/onlineIdeSample.csproj", "onlineIdeSample/"]
RUN dotnet restore "onlineIdeSample/onlineIdeSample.csproj"
COPY . .
WORKDIR "/src/onlineIdeSample"
RUN dotnet build "onlineIdeSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "onlineIdeSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "onlineIdeSample.dll"]

