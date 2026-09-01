# Learn about building .NET container images: https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG TARGETARCH
RUN echo "TARGETARCH: ${TARGETARCH}"

WORKDIR /source
RUN echo "HOME: ${HOME}"

# Copy project file and restore as distinct layers
COPY --link *.csproj .
RUN dotnet restore -a $TARGETARCH

FROM build AS publish
ARG TARGETARCH
# Copy source code and publish app
COPY --link . .
RUN dotnet publish -a $TARGETARCH --no-restore -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
ARG TARGETARCH

# RUN apt-get -y update; apt-get -y install curl; apt-get -y install iputils-ping  
RUN echo "HOME: ${HOME}"
WORKDIR /app
COPY --link --from=publish /app .
USER $APP_UID
RUN echo "APP_UID: ${APP_UID}"
RUN echo "HOME: ${HOME}"
ENTRYPOINT ["./Poc.Docker.Console"]
