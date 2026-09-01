# Learn about building .NET container images: https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID

ARG PROJECT

WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081

#RUN echo "APP_UID=${APP_UID}"
#RUN echo "HOME=${HOME}"

# ARG CERT_PATH # /https/aspnetapp.pfx
# ENV ASPNETCORE_URLS="https://+;http://+"
# ENV ASPNETCORE_HTTP_PORTS=8080
# ENV ASPNETCORE_HTTPS_PORTS=8081
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password="pwd"
# ENV ASPNETCORE_Kestrel__Certificates__Default__Path=$CERT_PATH

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release
ARG PROJECT

WORKDIR /src
COPY ./${PROJECT} ./

#RUN echo "BUILD_CONFIGURATION=${BUILD_CONFIGURATION}"
#RUN echo "PROJECT=${PROJECT}"
#RUN pwd
#RUN ls -al

RUN dotnet restore
RUN dotnet build --configuration "${BUILD_CONFIGURATION}" --output "/app/build"

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish

#RUN echo "BUILD_CONFIGURATION=${BUILD_CONFIGURATION}"
#RUN echo "PROJECT=${PROJECT}"
#RUN pwd
#RUN ls -al

RUN dotnet publish --no-restore --configuration "${BUILD_CONFIGURATION}" --output "/app/publish" --property:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .

#RUN echo "APP_UID=${APP_UID}"
#RUN echo "HOME=${HOME}"
#RUN pwd
#RUN ls -al

ENTRYPOINT ["dotnet", ""]