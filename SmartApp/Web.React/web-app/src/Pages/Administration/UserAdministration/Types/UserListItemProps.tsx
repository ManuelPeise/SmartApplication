export type UserListItemProps = {
  index: number;
  label: string;
  description?: string;
  disabled: boolean;
  onClick: () => void;
};
