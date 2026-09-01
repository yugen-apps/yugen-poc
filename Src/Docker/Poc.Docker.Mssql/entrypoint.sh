echo "...starting entrypoint..."

ls -al

/opt/mssql/bin/sqlservr & /usr/src/app/setup.sh & sleep infinity