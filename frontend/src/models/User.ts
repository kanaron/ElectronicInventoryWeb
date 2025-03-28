export interface User {
  userName: string;
  email: string;
  token: string;
  tmeToken?: string;
}

export interface UserFormValues {
  userName: string;
  password: string;
  email?: string;
}
