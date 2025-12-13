export interface MenuItem {
  title: string;
  icon?: string;
  route?: string;
  children?: MenuItem[];
  expanded?: boolean;
  disabled?: boolean;
  badge?: string;
  badgeColor?: 'primary' | 'accent' | 'warn';
  action?: string;  // Required permission action (e.g., 'View')
  resource?: string;  // Required permission resource (e.g., 'Products')
  isGroupHeader?: boolean;  // For sub-menu group headers
  pageStatus?: 'completed' | 'inProgress' | 'comingSoon';
}

export interface MenuSection {
  title: string;
  items: MenuItem[];
}
