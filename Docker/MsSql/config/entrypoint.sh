#!/bin/bash
echo "======= ENTRYPOINT STARTED ========"
waiting_time=60

# wait for MSSQL server to start
export STATUS=1
i=0

echo "======= MSSQL SERVER STARTED ========"
sleep $waiting_time/2
while [[ $STATUS -ne 0 ]] && [[ $i -lt $waiting_time/2 ]];
do
	i=$i+1
	/opt/mssql-tools18/bin/sqlcmd -t 1 -U sa -P MSSQL_SA_PASSWORD -Q "select 1" >> /dev/null
	STATUS=$?
done

if [ $STATUS -ne 0 ]; then 
	echo "======= Error: MSSQL SERVER took more than $waiting_time seconds to start up.  ========";
	exit 1
fi

/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P MSSQL_SA_PASSWORD -d master -i "init.sql";
echo "======= MSSQL CONFIG COMPLETE ======="