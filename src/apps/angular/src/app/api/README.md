# API Client Integration

This Angular project uses **NSwag** to generate a TypeScript API client from the backend's Swagger/OpenAPI specification, matching the pattern used in the Blazor client.

## File Structure

```
src/app/api/
├── index.ts                    # Module exports
├── api-client.generated.ts     # Auto-generated NSwag client (DO NOT EDIT)
└── README.md                   # This file
```

## Configuration

The NSwag configuration is located at: `angular/nswag.json`

Key settings:
- **Template**: Angular (uses HttpClient)
- **Output**: `src/app/api/api-client.generated.ts`
- **API URL**: `https://localhost:7000/swagger/v1/swagger.json`

## Generating the API Client

### Prerequisites

1. Install NSwag CLI globally (if not already installed):
   ```bash
   npm install -g nswag
   ```

2. Make sure the backend API is running at `https://localhost:7000`

### Generate the Client

Using npm script:
```bash
cd src/apps/angular
npm run generate-api
```

Or using the shell script:
```bash
./scripts/nswag-regen.sh
```

Or directly with nswag:
```bash
npx nswag run nswag.json
```

## Using the Generated Client

### 1. Import the Client

```typescript
import { Client, API_BASE_URL } from '@api';
// or
import { Client, API_BASE_URL } from '@app/api';
```

### 2. Configure in app.config.ts

The API_BASE_URL is configured as an InjectionToken. Update `app.config.ts`:

```typescript
import { API_BASE_URL } from '@api';
import { environment } from '@env/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    // ... other providers
    { provide: API_BASE_URL, useValue: environment.apiUrl }
  ]
};
```

### 3. Inject and Use in Components/Services

```typescript
import { Component, inject } from '@angular/core';
import { Client } from '@api';

@Component({...})
export class ProductsComponent {
  private apiClient = inject(Client);

  async loadProducts() {
    try {
      const result = await this.apiClient.searchProductsEndpointAsync({
        pageNumber: 1,
        pageSize: 10
      });
      console.log(result);
    } catch (error) {
      console.error('API Error:', error);
    }
  }
}
```

### 4. Using with Observables (RxJS)

The generated client returns Promises by default. To use with RxJS:

```typescript
import { from } from 'rxjs';

const products$ = from(this.apiClient.searchProductsEndpointAsync({...}));
```

## Generated Types

The NSwag client generates:

1. **Client Classes** - One client per API controller (e.g., `ProductsClient`, `UsersClient`)
2. **DTO Interfaces/Classes** - All request/response models
3. **Enums** - All enumerations from the API
4. **ApiException** - Error handling class

Example generated types:
```typescript
// Request models
interface CreateProductCommand { name: string; price: number; ... }
interface SearchProductsRequest { pageNumber: number; pageSize: number; ... }

// Response models
interface ProductResponse { id: string; name: string; ... }
interface ProductResponsePagedList { items: ProductResponse[]; totalCount: number; ... }
```

## Comparison with Blazor Client

| Feature | Blazor (`Client.cs`) | Angular (`api-client.generated.ts`) |
|---------|---------------------|-------------------------------------|
| HTTP Client | System.Net.Http.HttpClient | Angular HttpClient |
| Async Pattern | Task<T> | Promise<T> |
| DI Token | N/A | API_BASE_URL InjectionToken |
| Namespace | FSH.Starter.Blazor.Infrastructure.Api | N/A (ES modules) |
| Configuration | nswag.json (C# target) | nswag.json (TypeScript target) |

## Troubleshooting

### "Cannot connect to API"
- Ensure the API is running at `https://localhost:7000`
- Check if swagger.json is accessible at `https://localhost:7000/swagger/v1/swagger.json`

### "NSwag command not found"
- Install globally: `npm install -g nswag`
- Or use npx: `npx nswag run nswag.json`

### "Generated file has errors"
- Check if TypeScript version is compatible (5.0+)
- Ensure all Angular dependencies are installed
- Try regenerating after updating dependencies

## Offline Generation

If you can't connect to the running API, you can:

1. Download `swagger.json` when the API is available
2. Save it to `src/app/api/swagger.json`
3. Update `nswag.json`:
   ```json
   "documentGenerator": {
     "fromDocument": {
       "json": "src/app/api/swagger.json",
       "url": ""
     }
   }
   ```
4. Run generation
