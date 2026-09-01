echo "...starting setup..."

ls -al

until (echo select 1 from dual | /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P Password1 -d master > /dev/null)
do
 echo sleeping for mssql
 sleep 5
done

/opt/mssql-tools18/bin/sqlcmd -S localhost -U "sa" -P "Password1" -No -d master -i "setup.sql"