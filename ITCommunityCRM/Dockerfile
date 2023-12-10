FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything
COPY . .
WORKDIR /app/ITCommunityCRM
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o /app/publish

FROM scratch AS export-stage
COPY --from=build-env /app/publish /
