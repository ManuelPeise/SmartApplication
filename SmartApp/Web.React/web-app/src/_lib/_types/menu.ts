export type SideMenuEntry = {
  displayName: string;
  route: string;
  disabled: boolean;
  icon: JSX.Element;
};

export type SideMenu = {
  items: SideMenuEntry[];
};
