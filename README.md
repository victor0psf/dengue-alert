# Dengue Alert

Aplicação full-stack (ASP.NET Core + Angular) para consultar, armazenar e visualizar alertas epidemiológicos de dengue de Belo Horizonte (geocode `3106200`) fornecidos pela API pública [info.dengue](https://info.dengue.mat.br/).

## Visão geral

- **Propósito**: manter um histórico local (MySQL) dos últimos 6 meses de alertas e oferecer UI simples para sincronizar dados, listar as últimas semanas e buscar por semana epidemiológica específica.
- **Backend**: `AlertDengueApi` expõe REST API (`/api/dengue`) que consome o serviço externo e persiste os registros.
- **Frontend**: `AlertDengueFront-end` (Angular 20) consome a API interna para exibir dashboards rápidos.

## Arquitetura e fluxo

1. `POST /api/dengue/sync` calcula as últimas 6 semanas epidemiológicas via `EpidemologicalWeekHelper` e chama `https://info.dengue.mat.br/api/alertcity`.
2. `DengueAlertService` grava a resposta em MySQL via `DengueAlertRepository`.
3. O frontend usa `ApiService` para:
   - listar todos os alertas (`/get-all`) e mostrar as 3 semanas mais recentes,
   - buscar por semana (`/{week}`),
   - disparar sincronizações on-demand.
4. Swagger (`/swagger`) facilita o teste manual durante o desenvolvimento.

## Stack

| Camada     | Tecnologia / Versão                | Pasta                           |
|------------|------------------------------------|----------------------------------|
| Backend    | .NET 9, ASP.NET Core, EF Core, MySQL (Pomelo e Docker) | `AlertDengueApi/compose.yaml`                |
| Banco      | MySQL 8 (Docker)                   | `AlertDengueApi/compose.yaml`    |
| Frontend   | Angular 20, RxJS, standalone components | `AlertDengueFront-end`      |

## Pré-requisitos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- [Node.js 20 LTS](https://nodejs.org) + npm (Angular CLI ≥ 20 global opcional)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) para subir o MySQL localmente
- `dotnet-ef` (opcional para aplicar migrations manualmente): `dotnet tool install --global dotnet-ef`

## Configuração do backend

1. **Banco de dados e API**
   ```powershell
   cd AlertDengueApi
   docker compose up -d
Sobe um container mysql_alertdengue expondo 3307->3306.
Usuário padrão: usuario / alertdenguemysql (definido em appsettings.json).

cd AlertDengueApi
dotnet restore
dotnet ef database update
Se não tiver dotnet-ef instalado, use dotnet tool install --global dotnet-ef.

cd AlertDengueFront-end
npm install
npm run start
Servirá em http://localhost:4200.
