# Docker

# Learn about building .NET container images: https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

## Volumes

### Windows 

Update readd/write the folder permissions

### Linux

sudo chown $USER .volumes/ -R
sudo chmod 777 .volumes/ -R

## Https
https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-10.0
https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-10.0
https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md

### Windows
%USERPROFILE%

%APPDATA%\Microsoft\UserSecrets
%APPDATA%\ASP.NET\Https

docker-compose.yml

environment:
    - ASPNETCORE_HTTPS_PORTS=8443
ports:
    - "9999:8443"
volumes:
    - ${APPDATA}/microsoft/UserSecrets/:/home/app/.microsoft/usersecrets
    - ${APPDATA}/ASP.NET/Https:/home/app/.aspnet/https:ro

### Linux

dotnet dev-certs https -ep ${HOME}/.aspnet/https/localhost.pfx -p "Password@12345#"
dotnet dev-certs https --trust

~/.microsoft/usersecrets/
~/.aspnet/https/

docker-compose.yml

environment:
    - ASPNETCORE_HTTPS_PORTS=8443
    - ASPNETCORE_Kestrel__Certificates__Default__Password=Password@12345#
    - ASPNETCORE_Kestrel__Certificates__Default__Path=/home/app/.aspnet/https/localhost.pfx
ports:
    - "9999:8443"
volumes:
    - ${HOME}/.microsoft/usersecrets/:/home/app/.microsoft/usersecrets
    - ${HOME}/.aspnet/https/:/home/app/.aspnet/https:ro

or secrets.json

{
  "Kestrel": {
    "Certificates": {
      "Default": {
        "Password": "Password@12345#",
        "Path": "/home/app/.aspnet/https/localhost.pfx"
      }
    }
  }
}

ubuntu certs dirs

~/.aspnet/https/ 
localhost.pfx to add to asp.net app

/usr/local/share/ca-certificates/
dotnet-devcert.crt

/usr/lib/ssl/certs/
*.pem link to dotnet-devcert.crt (not needed?)
ca-certificates.crt (merged into?)

~/.dotnet/corefx/cryptography/x509stores/my/
thumb.pfx file (not needed?)

~/.aspnet/dev-certs/trust 
pem file (not needed)

### Asustor

/volume0/usr/builtin/etc/certificate

/volume1/.@plugins/AppCentral/letsencrypt/.CertBot/config/live/
(not needed)

1. settings > certificate manager > export certificate
2. vscode > X.509 Certificate Toolkit > Convert > Format Conversion Hub >
  - ssl.crt
  - ssl.key
  - p12 password
  - build > panda-nas-01.myasustor.com.pfx

/volume1/Docker/https/panda-nas-01.myasustor.com.pfx

### Unraid

/boot/config/ssl/certs > pem > pfx

/mnt/user/appdata/https/panda-nas.local.pfx

## Compose

### Windows

> docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.windows.yml up --detach

### Linux

> docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.linux.yml up --detach

### Asustor

> sudo docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.asustor.yml up --detach

### Unraid

> sudo docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.unraid.yml up --detach

## Poc.Docker.Aspnet

### Windows

> docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.windows.yml up --detach poc-docker-aspnet

> docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.windows.yml build poc-docker-aspnet

> docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.windows2.yml run --publish 9998:8443 --detach poc-docker-aspnet

### Linux

> docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.linux.yml up --detach poc-docker-aspnet

> docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.linux.yml build poc-docker-aspnet

> docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.linux2.yml run --publish 9998:8443 --detach poc-docker-aspnet --no-deps

### Asustor

> sudo docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.asustor.yml run --publish 3000:8443 --detach poc-docker-aspnet --no-deps

### Unraid

> sudo docker compose --file docker-compose/docker-compose.yml --file docker-compose/docker-compose.unraid.yml run --publish 3000:8443 --detach poc-docker-aspnet --no-deps


## Poc.Docker.MsSql

https://learn.microsoft.com/en-us/sql/linux/install-upgrade/quickstart-install-docker?view=sql-server-ver17&tabs=cli&pivots=cs1-bash
https://learn.microsoft.com/en-us/sql/linux/containers/configure?view=sql-server-ver17&pivots=cs1-bash

/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Password@12345#" -No

docker-compose.yml

environment:
    - ConnectionStrings__MsSql=Server=poc-docker-sql;User ID=sa;Password=Password@12345#;Encrypt=True;Trust Server Certificate=True;Connection Timeout=10;

or secrets.json

{
  "ConnectionStrings": {
    "MsSql": "Server=localhost,1433;User ID=sa;Password=Password@12345#;Encrypt=True;Trust Server Certificate=True;Connection Timeout=10;",
  }
}













# ASP.NET Core Docker Sample




This sample demonstrates how to build container images for ASP.NET Core web apps. See [.NET Docker Samples](../README.md) for more samples.

> [!NOTE]
> .NET 8+ container images use port `8080`, by default. Previous .NET versions used port `80`. The instructions for the sample assume the use of port `8080`.

## Run the sample image

You can start by launching a sample from our [container registry](https://mcr.microsoft.com/) and access it in your web browser at `http://localhost:8000`.

```console
docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 mcr.microsoft.com/dotnet/samples:aspnetapp
```

You should see the following console output as the application starts:

```console
> docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 aspnetapp
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://[::]:8080
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

After the application starts, navigate to `http://localhost:8000` in your web browser. You can also view the ASP.NET Core site running in the container from another machine with a local IP address such as `http://192.168.1.18:8000`.

You can also reach the app's endpoint from the command line:

```bash
$ curl http://localhost:8000/Environment
{"runtimeVersion":".NET 9.0.0-rc.1.24431.7","osVersion":"Debian GNU/Linux 12 (bookworm)","osArchitecture":"X64","user":"app","processorCount":16,"totalAvailableMemoryBytes":33632370688,"memoryLimit":9223372036854771712,"memoryUsage":35770368,"hostName":"834f365bfcfa"}
```

> [!NOTE]
> ASP.NET Core apps (in official images) listen to [port 8080 by default](https://github.com/dotnet/dotnet-docker/blob/6da64f31944bb16ecde5495b6a53fc170fbe100d/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7), starting with .NET 8. The [`-p` argument](https://docs.docker.com/engine/reference/commandline/run/#publish) in these examples maps host port `8000` to container port `8080` (`host:container` mapping). The container will not be accessible without this mapping. ASP.NET Core can be [configured to listen on a different or additional port](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints).

You can see the app running via `docker ps`.
The sample includes a [health check](https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks) endpoint at `/healthz`, indicated in the "STATUS" column.

```bash
$ docker ps
CONTAINER ID   IMAGE                                        COMMAND         CREATED          STATUS                    PORTS                  NAMES
d79edc6bfcb6   mcr.microsoft.com/dotnet/samples:aspnetapp   "./aspnetapp"   35 seconds ago   Up 34 seconds (healthy)   0.0.0.0:8080->8080/tcp   nice_curran
```

## Change port

You can change the port ASP.NET Core uses with one of the following environment variables.
However, the default port `8080` is recommended.

Note that ports 1 to 1023 are restricted to root users only, and will not work when running as the non-root root user provided in .NET images.

The following evnironment variables will change the port to port `80`.

- `ASPNETCORE_HTTP_PORTS=80`
- `ASPNETCORE_URLS=http://+:80`

`ASPNETCORE_URLS` overrides `ASPNETCORE_HTTP_PORTS` if set.
The `ASPNETCORE_HTTP_PORTS` envrionment variable is used in the [ASP.NET Core](https://github.com/dotnet/dotnet-docker/blob/d90e7bd1d10c8781f0008f5ab1327ca3481e78de/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7C5-L7C31)
images to set the default port.

## Enable HTTPS

To host the sample image with HTTPS, follow the instructions for [Running pre-built Container Images with HTTPS](../host-aspnetcore-https.md#hosting-aspnet-core-images-with-docker-over-https).
For a more in-depth guide to developing ASP.NET Core apps with HTTPS, check out [Developing ASP.NET Core Applications with Docker over HTTPS](../run-aspnetcore-https-development.md).

## Build the sample image

You can build the sample image using the following command (cloninig the repo isn't necessary):

```console
docker build --pull -t aspnetapp 'https://github.com/dotnet/dotnet-docker.git#:samples/aspnetapp'
```

If you have cloned the repo, you can build the image using your local copy:

```console
cd samples/aspnetapp
docker build --pull -t aspnetapp .
```

Add the argument `-f <Dockerfile>` to build the sample in a different configuration.
For example, build an [Ubuntu Chiseled](https://devblogs.microsoft.com/dotnet/dotnet-6-is-now-in-ubuntu-2204/#net-in-chiseled-ubuntu-containers) image using [Dockerfile.chiseled](Dockerfile.chiseled):

```console
docker build --pull -t dotnetapp -f Dockerfile.chiseled 'https://github.com/dotnet/dotnet-docker.git#:samples/dotnetapp'
```

You can run your local image the same way as described in [Run the sample image](#run-the-sample-image):

```console
docker run --rm -it -p 8000:8080 -e ASPNETCORE_HTTP_PORTS=8080 aspnetapp
```

### Multi-platform build

.NET sample Dockerfiles support multi-platform builds via cross-compilation.
First, check out the Docker [multi-platform build prerequisites](https://docs.docker.com/build/building/multi-platform/#prerequisites)

Once you have the prerequisites set up, you can build the ASP.NET app sample for a specific architecture:

```console
# From an amd64 machine:
docker buildx build --platform linux/arm64 -t aspnetapp .

# From an arm64 machine:
docker buildx build --platform linux/amd64 -t aspnetapp .
```

You can also build both platforms at once:

```console
docker buildx build --platform linux/amd64,linux/arm64 -t aspnetapp .
```

This works thanks to .NET's support for cross-compilation.
The build runs on your build machine's architecture and outputs IL for the target architecture.
The app is then copied to the final stage without running any commands on the target image - there's no emulation involved.

> [!NOTE]
> .NET does not support running under QEMU emulation. See [.NET and QEMU](../build-for-a-platform.md#net-and-qemu) for more information.

## Build image with the SDK

The easiest way to [build images is with the SDK](https://github.com/dotnet/sdk-container-builds).

```console
dotnet publish /p:PublishProfile=DefaultContainer
```

That command can be further customized to use a different base image and publish to a container registry. You must first use `docker login` to login to the registry.

```console
dotnet publish /p:PublishProfile=DefaultContainer /p:ContainerBaseImage=mcr.microsoft.com/dotnet/aspnet:9.0-noble-chiseled /p:ContainerRegistry=docker.io /p:ContainerRepository=youraccount/aspnetapp
```

## Supported Linux distros

The .NET Team publishes images for [multiple distros](../../documentation/supported-platforms.md).

Samples are provided for:

- [Alpine](Dockerfile.alpine)
- [Alpine with Composite ready-to-run image](Dockerfile.alpine-composite)
- [Alpine with ICU installed](Dockerfile.alpine-icu)
- [Debian](Dockerfile.debian)
- [Ubuntu](Dockerfile.ubuntu)
- [Ubuntu Chiseled](Dockerfile.chiseled)
- [Ubuntu Chiseled with Composite ready-to-run image](Dockerfile.chiseled-composite)

## Supported Windows versions

The .NET Team publishes images for [multiple Windows versions](../../documentation/supported-platforms.md). You must have [Windows containers enabled](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers) to use these images.

Samples are provided for

- [Nano Server](Dockerfile.nanoserver)
- [Windows Server Core](Dockerfile.windowsservercore)
- [Windows Server Core with IIS](Dockerfile.windowsservercore-iis)

Windows variants of the sample can be pulled via one the following registry addresses:

- `mcr.microsoft.com/dotnet/samples:aspnetapp-nanoserver-1809`
- `mcr.microsoft.com/dotnet/samples:aspnetapp-nanoserver-ltsc2022`
- `mcr.microsoft.com/dotnet/samples:aspnetapp-nanoserver-ltsc2025`
