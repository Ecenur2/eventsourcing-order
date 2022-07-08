FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Order.EventConsumer/Order.EventConsumer.csproj", "Order.EventConsumer/"]
RUN dotnet restore "Order.EventConsumer/Order.EventConsumer.csproj"
COPY . .
WORKDIR "/src/Order.EventConsumer"
RUN dotnet build "Order.EventConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.EventConsumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.EventConsumer.dll"]
