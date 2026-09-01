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
