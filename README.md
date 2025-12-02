# WineSoft Platform

Short notes to run the API locally and the integration tests for the IAM/signup feature.

Local run (Windows cmd.exe):

1. Export a temporary JWT secret (required for token generation):

```cmd
set JWT_SECRET=mi_jwt_secret_de_prueba_12345
dotnet run --project WinesoftPlatform.API\WinesoftPlatform.API.csproj
```

2. Default port is configured by ASP.NET (check console output). Use the curl below to test signup (it will save received cookies):

```cmd
curl -i -X POST http://localhost:5000/api/iam/authentication/signup -H "Content-Type: application/json" -d "{\"username\":\"test@example.com\",\"password\":\"P4ssword123\",\"displayName\":\"Test\"}" -c cookies.txt
```

Notes:
- The signup endpoint sets a cookie named `ws_auth` containing the JWT token.
- Cookie options: HttpOnly = true, SameSite = Strict, Secure = true in non-development environments; Expires = 1 hour.
- The API reads `JWT_SECRET` from environment variables. For local testing set it as shown above.
- BCrypt work factor can be configured via configuration key `IAM:BcryptWorkFactor` (default 10).

Running integration tests (Windows cmd.exe):

```cmd
cd tests\WinesoftPlatform.API.IntegrationTests
set JWT_SECRET=test_jwt_secret_12345
dotnet restore
dotnet test
```

EF Core / migrations (recommended steps):
- We added `IdentityDbContext` and an EF `UserRepository` under `IAM/Infrastructure/Persistence`.
- To create a migration you can run (adjust projects paths if needed):

```cmd
cd D:\Documentos\WebstormProjects\winesoft-platformss
set ASPNETCORE_ENVIRONMENT=Development
set IAM__UseEf=true
# Ensure Program.cs registers IdentityDbContext to a provider (e.g., MySQL or Sqlite) before running migrations
# Example using a design-time provider project or the API project as startup:
# dotnet ef migrations add AddIdentityUser --project WinesoftPlatform.API --startup-project WinesoftPlatform.API
# dotnet ef database update --project WinesoftPlatform.API --startup-project WinesoftPlatform.API
```

Swagger & CORS:
- The Swagger UI is available when running the API. The JWT is delivered in the `ws_auth` cookie (HttpOnly) so the frontend must use credentials (`fetch(url, { credentials: 'include' })`).
- CORS policy is configured to allow credentials. Adjust origins in `Program.cs` as needed.

Security / packages:
- `System.IdentityModel.Tokens.Jwt` was updated to a patched version to address a security advisory.

If anything is failing when running tests or starting locally, paste the terminal output here and lo reviso.
