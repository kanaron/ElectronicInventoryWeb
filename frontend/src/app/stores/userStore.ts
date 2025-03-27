import { makeAutoObservable, runInAction } from "mobx";
import { User, UserFormValues } from "../../models/User";
import agent from "../agent";
import { store } from "./store";
import { router } from "../router/Routes";

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
    store.commonStore.setToken(user.token);
    runInAction(() => (this.user = user));
    router.navigate("/");
  };

  logout = () => {
    store.commonStore.setToken(null);
    this.user = null;
    router.navigate("/");
  };

  getUser = async () => {
    try {
      const user = await agent.Account.current();
      runInAction(() => (this.user = user));
    } catch (error) {
      console.log(error);
    }
  };

  register = async (creds: UserFormValues) => {
    const user = await agent.Account.register(creds);
    store.commonStore.setToken(user.token);
    runInAction(() => (this.user = user));
    router.navigate("/");
    console.log(user);
  };

  updateTmeToken = async (token: string) => {
    if (!this.user) return;

    runInAction(() => {
      this.user!.tmeToken = token;
    });

    try {
      await agent.Account.updateTmeToken(this.user!);
    } catch (error) {
      console.error("Error updating TME token", error);
    }
  };
}
