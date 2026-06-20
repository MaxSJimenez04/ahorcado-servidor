# Servidor Ahorcado
## Servidor WCF (Windows Communication Fundation) del juego el Ahorcado (hangman)
---
Este proyecto es el servidor en WCF con ISS Express para la conexión a la base de datos y los siguientes servicios:
 * SesionService (Administrar sesiones de usuario)
 * UsuarioService (Administrar cuentas de usuario, modificar y crear)
 * EstadisticasService (Consultar las estadisticas globales e individuales)
 * PartidaService (Manejar el flujo de las partidas, y administrar las partidas existentes)
 * PalabraService (Obtener las palabras y las categorias de palabras)

### Requisitos Previos
* Windows Server (o Windows 10/11 Pro) con IIS instalado.
* .NET Framework (versión que use el proyecto) instalado.
* Características de Windows habilitadas:
    * Internet Information Services → World Wide Web Services → Application Development Features → ASP.NET.
    * .NET Framework Advanced Services → WCF Services:
        * HTTP Activation (necesaria para endpoints HTTP).
* Permisos de administrador para configurar IIS, el firewall y los servicios de Windows.
* MS SERVER 2022 o similar

### Clonar e instalar el repositorio
Este servidor se puede descargar y poner en marcha de forma independiente, sin necesidad del resto del juego (cliente, servidor de chat, etc.). Solo se necesita una carpeta vacía donde clonar, ya que el proyecto usa packages.config (formato clásico de NuGet) y restaura sus dependencias en una carpeta packages propia.

```bash
git clone https://github.com/MaxSJimenez04/ahorcado-servidor.git ServidorAhorcado
cd ServidorAhorcado
```

### Abrir el proyecto
No hace falta un archivo `.sln`: basta con abrir el `.csproj` directamente.
 
- Visual Studio → `File` → `Open` → `Project/Solution` → seleccionar `ServidorAhorcado.csproj`.

### Restaurar proyecto Nuget
El proyecto usa `packages.config`. Para que las dependencias se descarguen en la carpeta que el `.csproj` espera, agregar (una sola vez, ya versionado en el repo) un `nuget.config` dentro de `ServidorAhorcado/`:
 
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <config>
    <add key="repositoryPath" value="..\packages" />
  </config>
</configuration>
```
 
Con esto, NuGet crea automáticamente una carpeta `packages` **junto a** `ServidorAhorcado/` (un nivel arriba) — no necesita que ahí existan los demás proyectos, solo que esa carpeta padre exista (que ya existe, porque ahí clonaste el repo).
 
- **Desde Visual Studio:** al abrir el `.csproj`, la restauración automática (activada por defecto) descarga los paquetes solos.
- **Desde línea de comandos** (requiere `nuget.exe` en el `PATH`):
```bash
  nuget restore ServidorAhorcado.csproj
```

### Conectar a la base de datos
Crear una base de datos con el script de la base de datos, se puede dejar vacía o ejecutar el script de llenado de datos, una vez se ha creado, se debe crear un archivo `connectionStrings.config`, con la cadena de conexión de su Microsoft SQL Server
```xml
<connectionStrings>
  <add name="AhorcadoEntities"
       connectionString="metadata=res://*/Modelo.ModeloAhorcado.csdl|res://*/Modelo.ModeloAhorcado.ssdl|res://*/Modelo.ModeloAhorcado.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=TU_SERVIDOR\SQLEXPRESS;initial catalog=ahorcado_juego;persist security info=True;user id=TU_USUARIO;password=TU_PASSWORD;encrypt=False;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework&quot;"
       providerName="System.Data.EntityClient" />
</connectionStrings>
```
 
| Campo | Qué poner |
|---|---|
| `data source` | Servidor + instancia SQL, ej. `localhost\SQLEXPRESS` o `NOMBRE-PC\SQLEXPRESS` |
| `initial catalog` | Nombre de la base de datos (`ahorcado_juego`, no debería cambiar) |
| `user id` / `password` | Credenciales del usuario SQL en tu propia instancia |
 
> Para confirmar el `data source` exacto, abre **SQL Server Management Studio** y revisa el nombre del servidor al conectarte, o ejecuta `SELECT @@SERVERNAME;`.

## Conectar el servicio a la red local
 
Para que **otros equipos de la red** (no solo el propio servidor) puedan acceder al servicio, hay tres puntos clave:
 
### 1. Que IIS escuche en la IP correcta
 
En el binding del sitio (paso 3 anterior), usa `All Unassigned` (todas las interfaces) o la IP LAN del servidor, nunca `localhost`/`127.0.0.1`.
 
### 2. Abrir el puerto en el Firewall de Windows
 
```powershell
# Ejemplo: abrir el puerto 8080 para HTTP
New-NetFirewallRule -DisplayName "WCF Service HTTP" -Direction Inbound -Protocol TCP -LocalPort 8080 -Action Allow
 
# Ejemplo: abrir el puerto 808 para net.tcp
New-NetFirewallRule -DisplayName "WCF Service NetTcp" -Direction Inbound -Protocol TCP -LocalPort 808 -Action Allow
```
 
También puedes hacerlo desde `Firewall de Windows Defender con seguridad avanzada` → `Reglas de entrada` → `Nueva regla` → `Puerto`.
 
## Consumir el servicio desde otro equipo
 
Desde cualquier PC de la misma red, el servicio queda disponible en:
 
```
http://<IP-DEL-SERVIDOR>:<puerto>/Servicio.svc          # endpoint HTTP/SOAP
http://<IP-DEL-SERVIDOR>:<puerto>/Servicio.svc?wsdl      # metadatos (WSDL)
net.tcp://<IP-DEL-SERVIDOR>:<puerto>/Servicio.svc        # endpoint TCP (si está habilitado)
```
 
Por ejemplo: `http://192.168.1.50:8080/Servicio.svc`.