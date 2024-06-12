import { IUser } from "../interfaces/user.interface";

export class UserModel implements IUser {
    id: string;
    email: string;
    passwordHash: string;
    username: string;

    constructor(data: IUser){
        this.id = data.id;
        this.email = data.email;
        this.passwordHash = data.passwordHash;
        this.username = data.username;
    }
  }