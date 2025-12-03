export interface Product {
  id: string;
  name: string;
  description?: string;
  sku?: string;
  barcode?: string;
  price: number;
  cost?: number;
  quantity: number;
  reorderLevel?: number;
  categoryId?: string;
  categoryName?: string;
  brandId?: string;
  brandName?: string;
  imageUrl?: string;
  isActive: boolean;
  createdOn?: string;
  lastModifiedOn?: string;
}

export interface ProductListFilter {
  search?: string;
  categoryId?: string;
  brandId?: string;
  minPrice?: number;
  maxPrice?: number;
  isActive?: boolean;
}

export interface Category {
  id: string;
  name: string;
  description?: string;
  parentId?: string;
  parentName?: string;
  productsCount?: number;
  isActive: boolean;
}

export interface Brand {
  id: string;
  name: string;
  description?: string;
  logoUrl?: string;
  productsCount?: number;
  isActive: boolean;
}
