/**
 * API Module Exports
 * 
 * This module provides the auto-generated NSwag API client for Angular.
 * 
 * Usage:
 * 1. Run `./scripts/nswag-regen.sh` to generate the API client
 * 2. Import the generated client classes from this module
 * 
 * Example:
 * ```typescript
 * import { Client, API_BASE_URL } from '@app/api';
 * 
 * @Component({...})
 * export class MyComponent {
 *   private client = inject(Client);
 *   
 *   async loadData() {
 *     const result = await this.client.searchProductsEndpointAsync(...);
 *   }
 * }
 * ```
 */

// Export from the generated file
// After running nswag, this will export all the generated types
export * from './api-client.generated';

// Re-export common types that will be available after generation
// These are commented out until the actual file is generated
// export { Client, API_BASE_URL, ApiException } from './api-client.generated';
