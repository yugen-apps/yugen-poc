az group create --name "rg-tmp2" --location "westeurope"

az deployment group create --resource-group "rg-tmp2" --template-file "app.json"  --parameters "app.parameters.json"