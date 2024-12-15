export interface User {
  username: string;
  displayName: string;
  token: string;
}

export interface UserFormValues {
  login: string;
  password: string;
  displayName?: string;
  userName?: string;
}
