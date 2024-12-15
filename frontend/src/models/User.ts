export interface User {
  username: string;
  displayName: string;
  token: string;
}

export interface UserFormValues {
  userName: string;
  password: string;
  displayName?: string;
}
