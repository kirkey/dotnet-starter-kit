export interface ApiResponse<T> {
  succeeded: boolean;
  data?: T;
  messages?: string[];
  source?: string;
  exception?: string;
  errorId?: string;
  supportMessage?: string;
  statusCode?: number;
}

export interface PaginationFilter {
  pageNumber: number;
  pageSize: number;
  orderBy?: string[];
  keyword?: string;
}

export interface PaginatedResult<T> {
  items: T[];
  currentPage: number;
  totalPages: number;
  totalCount: number;
  pageSize: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
