import { Express } from "express";
import { Db, MongoClient } from 'mongodb'
import crypto from 'crypto';
import * as argon2 from "argon2";
import { Account } from "../models/Account";


export class Authentication {

    server?: Express;
    db?: Db;

    constructor() {

    }


    async Init(server: Express, db: Db) {
        this.server = server;
        this.db = db;

        this.server.post('/login', async (req, res) => {

            let { username, password } = req.body;

            console.log('login ' + username);
            if (username === null || password === null) {
                res.send('Invalid credentials')
            }

            let account = await db.collection('accounts').findOne<Account>({ username: username })
            if (account != null) {
                argon2.verify(account.password, password).then(async (success) => {
                    if (success) {
                        let response: any = { ...account };
                        delete response.password;
                        delete response.salt;
                        response.lastAuthentication = Date.now();
                        await db.collection('accounts').updateOne(
                            { "_id": account?._id },
                            { $set: { lastAuthentication: response.lastAuthentication } }
                        )
                        res.send(response)
                    }
                    else {
                        res.statusCode = 401;
                        res.send('Invalid credentials')
                    }
                })
            }
            else {
                res.statusCode = 401;
                res.send('Invalid credentials')
            }


        })

        this.server.post('/register', async (req, res) => {
            let { username, password } = req.body;
            if (username === null || password === null) {
                res.send('Invalid credentials')
            }

            let account = await db.collection('accounts').findOne({ username: username });
            if (account == null) {
                console.log('Creating account for ' + username);

                crypto.randomBytes(32, (err, salt) => {
                    if (err) {
                        console.log(err);
                    }
                    argon2.hash(password, { salt: salt }).then(async (hash) => {
                        let registration = new Account(username, hash, salt);
                        await db.collection('accounts').insertOne(registration);
                        let response: any = { ...registration };
                        delete response.password;
                        delete response.salt;
                        res.send(response);
                    })
                })


            }
            else {
                console.log('register already exist', account)
                res.statusCode = 403
                res.send('Already exist')
            }
        });

        console.log('Authentication::Init')
    }
}