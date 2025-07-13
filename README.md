# Plataforma de Admisión en Línea

Este repositorio contiene el código fuente de una plataforma web desarrollada como parte de una tesis de ingeniería de sistemas, cuyo objetivo es optimizar el proceso de admisión en línea en una institución especializada en diagnóstico y tratamiento de habilidades cognitivas.

## 🧠 Descripción

La plataforma permite a los usuarios:

- Registrarse de manera autónoma.
- Seleccionar evaluaciones o tratamientos clínicos.
- Subir comprobantes o realizar pagos en línea.
- Revisar historial clínico y gestionar citas.

Desarrollada con arquitectura en capas y enfoque modular, se diseñó para facilitar la adaptabilidad por especialidad y modalidad (presencial o virtual).

## 🔧 Tecnologías utilizadas

- ASP.NET Core 8
- MVC
- Entity Framework Core
- SQL Server 2019
- Bootstrap 5
- JavaScript
- OAuth 2.0 (Google/Microsoft login)
- Visual Studio 2022

## 🚀 Ejecución local

1. Clona el repositorio:
   ```bash
   git clone https://github.com/Billy2301/plataforma-admision-linea.git
   ```

2. Abre la solución `.sln` en Visual Studio.

3. Configura la cadena de conexión a tu SQL Server en `appsettings.json`.

4. Ejecuta la migración de base de datos:
   ```bash
   Update-Database
   ```

5. Inicia el proyecto desde Visual Studio (F5).

## 📁 Estructura del proyecto

- `PortalClienteV2/`: Proyecto web principal
- `BL/`: Lógica de negocio
- `DA/`: Acceso a datos
- `Entity/`: Entidades y modelos
- `IOC/`: Inyección de dependencias

## ⚠️ Aviso

Este repositorio contiene una versión académica de la solución. Algunas funciones sensibles, datos de conexión y lógica interna han sido modificadas o eliminadas por razones de confidencialidad.

## 📜 Licencia

Este proyecto fue desarrollado con fines académicos.
