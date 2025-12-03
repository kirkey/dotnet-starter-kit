export interface MenuItem {
  title: string;
  icon?: string;
  route?: string;
  children?: MenuItem[];
  expanded?: boolean;
  disabled?: boolean;
  badge?: string;
  badgeColor?: 'primary' | 'accent' | 'warn';
}

export interface MenuSection {
  title: string;
  items: MenuItem[];
}
