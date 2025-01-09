import { IconProp } from '@fortawesome/fontawesome-svg-core';

export interface SidebarLink {
  path: string;
  title: string;
  icon: IconProp;
}

export const SIDEBAR_LINKS: SidebarLink[] = [
  {
    path: 'home',
    title: 'Dashboard',
    icon: 'chart-area',
  },
  {
    path: 'config/person',
    title: 'Personas',
    icon: 'users',
  },
];
