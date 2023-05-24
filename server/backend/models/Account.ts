import { v4 as uuidv4 } from 'uuid';
import { Db, ObjectId } from 'mongodb'
export class Account {
    _id: ObjectId | undefined;
    username: string;
    password: string;
    salt: Buffer;

    lastAuthentication: number;


    constructor(username: string, password: string, salt: Buffer) {
        this._id = new ObjectId();
        this.username = username;
        this.password = password;
        this.salt = salt;

        this.lastAuthentication = Date.now();
    }
}