# API de Autenticación

Este proyecto implementa un microservicio de autenticación y gestión de usuarios utilizando C# y .NET 8, siguiendo principios SOLID y patrones de diseño.

## Características

- Arquitectura en capas (API, Aplicación, Dominio, Infraestructura)
- Procedimientos almacenados para todas las operaciones de base de datos
- Manejo centralizado de excepciones
- Documentación con Swagger
- Validación de datos con FluentValidation

## Requisitos

- .NET 8 SDK
- SQL Server

## Configuración

1. Actualizar la cadena de conexión en `appsettings.json` y `appsettings.Development.json`

## Procedimientos Almacenados

Todos los accesos a datos se realizan mediante procedimientos almacenados:

### Usuarios
- `SP_ObtenerUsuarioPorDocumento`