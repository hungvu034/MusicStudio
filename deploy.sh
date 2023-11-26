# Change these four parameters as needed
 ACI_PERS_RESOURCE_GROUP=hlam-rg
ACI_PERS_STORAGE_ACCOUNT_NAME=mystorageaccount$RANDOM
ACI_PERS_LOCATION=eastus
ACI_PERS_SHARE_NAME=acishare

# # Create the storage account with the parameters
az storage account create \
    --resource-group $ACI_PERS_RESOURCE_GROUP \
    --name $ACI_PERS_STORAGE_ACCOUNT_NAME \
    --location $ACI_PERS_LOCATION \
    --sku Standard_LRS

# # Create the file share
az storage share create \
  --name $ACI_PERS_SHARE_NAME \
  --account-name $ACI_PERS_STORAGE_ACCOUNT_NAME


echo $ACI_PERS_STORAGE_ACCOUNT_NAME

STORAGE_KEY=$(az storage account keys list --resource-group $ACI_PERS_RESOURCE_GROUP --account-name $ACI_PERS_STORAGE_ACCOUNT_NAME --query "[0].value" --output tsv)
echo $STORAGE_KEY

az container create \
    --resource-group $ACI_PERS_RESOURCE_GROUP \
    --name musicdata \
    --memory 3 \
    --ip-address public \
    --image mcr.microsoft.com/mssql/server:2019-latest \
    --environment-variables MSSQL_SA_PASSWORD=Password789 ACCEPT_EULA=Y \
    --ports 1433 \ 
   --azure-file-volume-account-name mystorageaccount30661 \
   --azure-file-volume-account-key DqZXeEx0HCY3k1456p8aY0O7htw4IuZzxkDZnWxC1/OHw9wLr1ZWGtbvl29uJRL2yALAzNtbgLlB+AStRhQ+xQ==\
   --azure-file-volume-share-name acishare \
    --azure-file-volume-mount-path /var/opt/mssql \


#Create the file share
az storage share create \
  --name mediashare \
  --account-name mystorageaccount30661


az container create \
    --resource-group $ACI_PERS_RESOURCE_GROUP \
    --name musicwebapp \
    --memory 3 \
    --ip-address public \
    --image mstudio.azurecr.io/hello-world \
    --environment-variables ASPNETCORE_ENVIRONMENT=Development ConnectionString="Data Source=20.239.5.225,1433;Initial Catalog=MusicStudio;User ID=SA;Password=Password789;TrustServerCertificate=True;" \
    --ports 80 443 \
    --azure-file-volume-account-name mystorageaccount30661 \
    --azure-file-volume-account-key DqZXeEx0HCY3k1456p8aY0O7htw4IuZzxkDZnWxC1/OHw9wLr1ZWGtbvl29uJRL2yALAzNtbgLlB+AStRhQ+xQ== \
    --azure-file-volume-share-name mediashare \
    --azure-file-volume-mount-path /app/wwwroot


# # Query IP 

ACI_IP=$(az container show \
  --name appcontainer \
  --resource-group myResourceGroup \
  --query ipAddress.ip --output tsv)

 az storage file upload-batch -d mediashare -s ./wwwroot --destination-path ./ --account-name mystorageaccount30661
