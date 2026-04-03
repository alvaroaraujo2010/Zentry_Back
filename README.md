# Zentry Backend - C# + MySQL + Clean Architecture

Backend regenerado desde cero para **MySQL 8**, alineado al script `zentry_full_database.sql`
y a la arquitectura del instructivo. La estructura sigue el enfoque modular/monolito recomendado
para un MVP vendible con multi-tenancy por `tenant_id`. fileciteturn12file0

## Estructura
- `Zentry.Api`
- `Zentry.Application`
- `Zentry.Domain`
- `Zentry.Infrastructure`

## Tablas modeladas
- tenants
- branches
- roles
- users
- user_branches
- refresh_tokens
- otp_codes
- customers
- staff_profiles
- catalog_services
- memberships
- customer_memberships
- appointments
- appointment_services
- invoices
- payments
- cash_sessions
- cash_movements
- subscriptions
- reminder_queue
- whatsapp_logs
- audit_logs

## Requisitos
- .NET 8 SDK
- MySQL 8
- Haber corrido previamente `zentry_full_database.sql`

## Configuración
Edita `src/Zentry.Api/appsettings.Development.json`

## Arranque
```bash
dotnet clean
dotnet restore
dotnet build
dotnet run --project .\src\Zentry.Api\Zentry.Api.csproj
```

## Nota honesta
Este entorno no tiene `dotnet`, así que no pude compilarlo aquí. El proyecto sí fue generado ya
alineado a MySQL y al script/base que compartiste.
