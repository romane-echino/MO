"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.Authentication = void 0;
const crypto_1 = __importDefault(require("crypto"));
const argon2 = __importStar(require("argon2"));
const Account_1 = require("../models/Account");
class Authentication {
    constructor() {
    }
    Init(server, db) {
        return __awaiter(this, void 0, void 0, function* () {
            this.server = server;
            this.db = db;
            this.server.post('/login', (req, res) => __awaiter(this, void 0, void 0, function* () {
                let { username, password } = req.body;
                console.log('login ' + username);
                if (username === null || password === null) {
                    res.send('Invalid credentials');
                }
                let account = yield db.collection('accounts').findOne({ username: username });
                if (account != null) {
                    argon2.verify(account.password, password).then((success) => __awaiter(this, void 0, void 0, function* () {
                        if (success) {
                            let response = Object.assign({}, account);
                            delete response.password;
                            delete response.salt;
                            response.lastAuthentication = Date.now();
                            yield db.collection('accounts').updateOne({ "_id": account === null || account === void 0 ? void 0 : account._id }, { $set: { lastAuthentication: response.lastAuthentication } });
                            res.send(response);
                        }
                        else {
                            res.statusCode = 401;
                            res.send('Invalid credentials');
                        }
                    }));
                }
                else {
                    res.statusCode = 401;
                    res.send('Invalid credentials');
                }
            }));
            this.server.post('/register', (req, res) => __awaiter(this, void 0, void 0, function* () {
                let { username, password } = req.body;
                if (username === null || password === null) {
                    res.send('Invalid credentials');
                }
                let account = yield db.collection('accounts').findOne({ username: username });
                if (account == null) {
                    console.log('Creating account for ' + username);
                    crypto_1.default.randomBytes(32, (err, salt) => {
                        if (err) {
                            console.log(err);
                        }
                        argon2.hash(password, { salt: salt }).then((hash) => __awaiter(this, void 0, void 0, function* () {
                            let registration = new Account_1.Account(username, hash, salt);
                            yield db.collection('accounts').insertOne(registration);
                            let response = Object.assign({}, registration);
                            delete response.password;
                            delete response.salt;
                            res.send(response);
                        }));
                    });
                }
                else {
                    console.log('register already exist', account);
                    res.statusCode = 403;
                    res.send('Already exist');
                }
            }));
            console.log('Authentication::Init');
        });
    }
}
exports.Authentication = Authentication;
