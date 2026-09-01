# Learn about building .NET container images: https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID

ARG PROJECT_DIR
ARG PROJECT_NAME
ENV PROJECT_NAME_DLL="${PROJECT_NAME}.dll"

WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release
ARG PROJECT_DIR
ARG PROJECT_NAME

WORKDIR /src
COPY ["${PROJECT_DIR}/${PROJECT_NAME}", "./"]

RUN dotnet restore
RUN dotnet build --configuration "${BUILD_CONFIGURATION}" \
                 --output "/app/build"

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish

RUN dotnet publish --configuration "${BUILD_CONFIGURATION}" \
                   --output "/app/publish" \
                   --property:UseAppHost=false \
                   --no-restore

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .

# Copies your code file from your action repository to the filesystem path `/` of the container
COPY ./Docker/entrypoint.sh /app/entrypoint.sh

# Code file to execute when the docker container starts up (`entrypoint.sh`)
ENTRYPOINT ["sh", "/app/entrypoint.sh"]
