# learning notes

## Migration create



```bash
# Install dotnet-ef for creating migration, run migration
dotnet tool install --global dotnet-ef

# Create Migration
dotnet ef migrations add InitialCreate -p DataAccess -s WebApi

# Run Migration
dotnet ef database update -p DataAccess -s WebApi
```