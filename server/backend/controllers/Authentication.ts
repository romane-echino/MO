import { Express } from "express";
import { Db } from 'mongodb'
import socketIO from 'socket.io'

export class Authentication {

    server?:Express;
    db?:Db;

    constructor(){
       
    }


    Init(server: Express, db:Db){
        this.server = server;
        this.db = db;

        this.server.post('/login', (req, res) => {
           let {username, password} = req.body;
        })
    }
}