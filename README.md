# eShopOnWeb — Application Security Portfolio Project

This is a fork of [Microsoft's eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb) (via NimblePros), a reference ASP.NET Core application built with Clean Architecture. I used it as a hands-on project to bridge my .NET development background into Application Security / DevSecOps.

**Everything below marked as "added" or "fixed" is my own work, built and documented while learning. The original application code and architecture belong to Microsoft/NimblePros — I did not build eShopOnWeb itself.**

## What I built

### New feature: Top Expensive Catalog Items endpoint
Added a new read endpoint following the project's existing Specification + FastEndpoints pattern:

- `GET /api/catalog-items-top-expensive` — returns the 5 highest-priced catalog items, sorted descending
- New files: `TopExpensiveCatalogItemsSpecification.cs`, `GetTopExpensiveCatalogItemsEndpoint.cs`, `GetTopExpensiveCatalogItemsEndpoint.GetTopExpensiveCatalogItemsResponse.cs`
- Covered by a unit test (Specification logic, xUnit — 2/2 passing) and an integration test (live endpoint behaviour, MSTest)

## Security work

### 1. Static Application Security Testing (SAST) — SonarQube Cloud
Ran SonarQube Cloud against the full codebase. Findings included hardcoded credentials, an over-permissive database network configuration in the Azure Bicep infra files, and unpinned CI/CD action dependencies. Security rating: **A**, 732+ lines analyzed.

### 2. Software Composition Analysis (SCA) — dependency vulnerability
VS Code's built-in NuGet scanning (and confirmed via SonarQube) flagged **AutoMapper 12.0.1** as carrying a known high-severity vulnerability ([GHSA-rvv3-g6hj-g44x](https://github.com/advisories/GHSA-rvv3-g6hj-g44x)) — a denial-of-service risk from unbounded recursive mapping.

The maintainer's patched versions (15.1.1+) are no longer free/open-source. Rather than accept new licensing cost or ship a known-vulnerable dependency, I **removed AutoMapper entirely** and replaced its usage with manual object mapping across 5 files (`CatalogBrandListEndpoint`, `CatalogTypeListEndpoint`, `CatalogItemListPagedEndpoint`, `MappingProfile` deleted, `Program.cs` registration removed). Verified all affected endpoints still return correct data after the change.

### 3. Hardcoded secret remediation
SonarQube flagged a database password committed in plaintext in `appsettings.Docker.json` and `docker-compose.yml`. Fixed by:
- Moving the real password into a `.env` file (excluded via `.gitignore`, never committed)
- Referencing it in `docker-compose.yml` via `${DB_PASSWORD}` variable substitution
- Passing it into both app containers via .NET's `ConnectionStrings__*` environment-variable override convention, so the checked-in `appsettings.Docker.json` now contains only a harmless placeholder

### 4. Dynamic Application Security Testing (DAST) — OWASP ZAP
Ran an OWASP ZAP baseline scan against the running application (`localhost:5106`). Result: **0 failures, 12 warnings**, mostly missing standard security headers (CSP, X-Content-Type-Options, anti-clickjacking, Permissions-Policy) and one outdated frontend JS library (Bootstrap).

Automated this scan into a **GitHub Actions pipeline** (`.github/workflows/zap-scan.yml`) that builds and starts the full application stack, runs the ZAP scan, and reports results automatically on every push to `main`.

## Tech stack
- .NET 10, ASP.NET Core MVC + FastEndpoints (two API styles in one solution)
- Entity Framework Core, Specification pattern (Ardalis.Specification), Mediator pattern
- Docker / Docker Compose (SQL Server, Web, PublicApi containers)
- SonarQube Cloud (SAST), OWASP ZAP (DAST), GitHub Actions (CI/CD)

## Running locally
```bash
docker-compose up -d --build
```
Web storefront: `http://localhost:5106`
Public API + Swagger: `http://localhost:5200/swagger`

Requires a `.env` file in the project root with:
## What's next
- OWASP Top 10 hands-on labs (PortSwigger Web Security Academy)
- CompTIA Security+ certification
- Fixing the missing security headers flagged by ZAP
