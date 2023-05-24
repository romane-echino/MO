"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Account = void 0;
const mongodb_1 = require("mongodb");
class Account {
    constructor(username, password, salt) {
        this._id = new mongodb_1.ObjectId();
        this.username = username;
        this.password = password;
        this.salt = salt;
        this.lastAuthentication = Date.now();
    }
}
exports.Account = Account;
