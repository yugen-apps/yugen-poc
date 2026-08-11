# https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver17&tabs=cli&pivots=cs1-bash
# https://hub.docker.com/r/microsoft/mssql-server
# Use the official SQL Server image
FROM mcr.microsoft.com/mssql/server:2025-latest AS sqlserver

WORKDIR /usr/src/app
CMD ["/bin/bash", "./entrypoint.sh"]

COPY Docker.Mssql/setup.sql .
COPY --chmod=755 Docker.Mssql/setup.sh .
COPY --chmod=755 Docker.Mssql/entrypoint.sh .