import { makeAutoObservable } from "mobx";
import { User, UserFormValues } from "../../models/User";
import agent from "../agent";

export default class UserStore {
  user: User | null = null;

  constructor() {
    makeAutoObservable(this);
  }

  get isLoggedIn() {
    return !!this.user;
  }

  login = async (creds: UserFormValues) => {
    const user = await agent.Account.login(creds);
    console.log(user);
  };
}
