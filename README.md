# bg-technical-test
Prueba Técnica_Desarrollador Fullstack .Net + Angular

# Migrations 
Para ejecutar las migraciones se debe ejecutar el siguiente comando en la consola de visual studio code:
```dotnet ef --startup-project ../../BG/BG.API migrations add InitialModel -o Data/Migrations```
Para aplicar las migraciones se debe ejecutar el siguiente comando en la consola de visual studio code:
```dotnet ef --startup-project ../../BG/BG.API database update```

# Swagger
Para visualizar la documentación de la API se debe ejecutar el proyecto y acceder a la siguiente URL:
```https://localhost:7088/openapi/v1.jsonl```