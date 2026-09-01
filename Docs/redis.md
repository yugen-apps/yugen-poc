# .NET and Redis on Azure Container Apps

This sample will show you how to create a multi-container app topology in [Azure Container Apps](https://learn.microsoft.com/en-us/azure/container-apps/) using the [Azure Developer CLI](https://aka.ms/azd/install). Here's a 5-minute video that walks you through a demo of using the repo to set it up yourself. 

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/ob8c2pQVw7c/0.jpg)](https://www.youtube.com/embed/ob8c2pQVw7c)

[![Open in GitHub Codespaces](https://img.shields.io/static/v1?style=for-the-badge&label=GitHub+Codespaces&message=Open&color=lightgrey&logo=github)](https://codespaces.new/bradygaster/dotnet-redis-pubsub)
[![Open in Dev Container](https://img.shields.io/static/v1?style=for-the-badge&label=Dev+Container&message=Open&color=blue&logo=visualstudiocode)](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/bradygaster/dotnet-redis-pubsub)

## Getting started

1. **📤 One-click setup**: [Open a new Codespace](https://codespaces.new/bradygaster/dotnet-redis-pubsub), giving you a fully configured cloud developer environment.
3. **▶️ Run, one-click again**: Use VS Code's built-in *Run* command and open the forwarded port *8080* in your browser.
5. **🔄 Iterate quickly:** Codespaces updates the server on each save, and VS Code's debugger lets you dig into the code execution.

## Run

This repository can be run in Codespaces, in Dev Containers, or locally on your development machine. 

### Run in Codespaces

1. Click here to open in GitHub Codespaces

    [![Open in GitHub Codespaces](https://img.shields.io/static/v1?style=for-the-badge&label=GitHub+Codespaces&message=Open&color=lightgrey&logo=github)](https://codespaces.new/bradygaster/dotnet-redis-pubsub)

### Run in Dev Container

1. Click here to open in Dev Container

    [![Open in Dev Container](https://img.shields.io/static/v1?style=for-the-badge&label=Dev+Container&message=Open&color=blue&logo=visualstudiocode)](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/bradygaster/dotnet-redis-pubsub)


### Run Locally

1. Clone the repo to your local machine `git clone https://github.com/bradygaster/dotnet-redis-pubsub`
1. Open repo in VS Code

## Then...

1. Open the `docker-compose.yml` file
1. Right-click anywhere in the file and select **Docker Compose - Up**
1. Switch to the Docker extension to see the containers start up
1. Right-click the `publisher` container and select the `Open in Browser` menu item
1. You should see `Poc.Redis.Publisher is up` in your browser
1. Add the URL slug `/swagger` to the end of the URL in your browser to open the Swagger UI test page
1. Open the `POST` method, and replace the JSON body in the test window with this:

    ```json
    {
      "date": "2023-06-10T04:52:16.323Z",
      "temperatureC": 30,
      "summary": "Hot"
    }
    ```
1. Right-click the `subscriber` container in the Docker containers pane, and select `View logs`
1. The logs should open up in your terminal window, and you should see evidence that the `subscriber` is receiving messages from the Redis subscription:

    ```
    info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
    info: Microsoft.Hosting.Lifetime[0]
        Hosting environment: Production
    info: Microsoft.Hosting.Lifetime[0]
        Content root path: /app
    info: Program[0]
        Message received from test-channel : Hot (85) at 06/10/2023 04:52:16
    info: Program[0]
        Message received from test-channel : Hot (85) at 06/10/2023 04:52:16
    ```

1. Send a few more messages using the Swagger UI tool whilst viewing the subscriber's logs in the editor

## Deploy to Azure

You can deploy the app to Azure using the Azure Developer CLI, from any of the environments above.

> NOTE: If you are running locally, then you first need to [install the Azure Developer CLI](https://aka.ms/azd/install)

### Deploy with Azure Developer CLI

1. Open a terminal
1. Run `azd auth login`
1. Run `azd up`

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.


# Redis: Implementing Pub/Sub and Streams in .NET 10

![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg) ![.NET](https://img.shields.io/badge/.NET-10-purple) ![Redis](https://img.shields.io/badge/Redis-Message%20Broker-red) ![Docker](https://img.shields.io/badge/Docker-Compose-blue)

> **Article:** [Redis: Implementing Pub/Sub and Streams in .NET 10](https://joaooliveira.net/en/blog/2026/01/redis-message-broker/)

This project demonstrates how to use **Redis as a message broker** with **Pub/Sub** and **Streams** in a **.NET 10 Minimal API**.

The goal is to compare both approaches in practice, showing how they behave, when to use each one, and the trade-offs between **real-time delivery** and **message durability**.

---

## What This Project Covers

- Redis **Pub/Sub** for real-time, fire-and-forget messaging
- Redis **Streams** for persistent, reliable message processing
- Background workers consuming messages
- Minimal API endpoints for publishing and processing messages
- Docker-based local environment with Redis

---

## Concepts Overview

### Pub/Sub

- Instant message delivery
- Messages are **not persisted**
- Consumers must be online to receive messages
- Ideal for real-time notifications and presence updates

### Streams

- Messages are **stored in Redis**
- Consumers can process messages later
- Supports ordering and durability
- Ideal for audit logs, order processing, and event-driven workflows

---

## Tech Stack

- **.NET 10**
- **.NET Minimal API**
- **Redis**
- **StackExchange.Redis**
- **Docker & Docker Compose**

---

## Project Structure

```text
.
├── .gitignore
├── docker-compose.yml
├── README.md
└── RedisMessageLabApi/
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── GlobalUsings.cs
    ├── Program.cs
    ├── RedisMessageLabApi.csproj
    ├── Interfaces/
    │   └── IRedisService.cs
    ├── Models/
    │   └── AppMessage.cs
    ├── Services/
    │   └── RedisService.cs
    ├── Workers/
    │   └── LiveNotificationWorker.cs
    ├── Properties/
```

## Environment Setup

Prerequisites

- .NET 10 SDK
- Docker & Docker Compose

Start Redis

```bash
docker-compose up -d
```

Run the API

``` bash
dotnet run
```

The API will be available at:

```bash
http://localhost:5040
```

---

## About

This repository is part of my technical writing and learning notes.  
If you found it useful, consider starring the repo and sharing feedback.

- Author: Joao Oliveira
- Blog: https://joaooliveira.net
- Topics: .NET, Redis, backend engineering, system design

## Contributing

Issues and pull requests are welcome.  
If you plan a larger change, please open an issue first so we can align on scope.

## License

Licensed under the **MIT License**. See the `LICENSE` file for details.
