import express from 'express'
import path from 'path'
import http from 'http'
import socketIO from 'socket.io'
import { v4 as uuidv4 } from 'uuid';

import { Ennemy, EnnemyEventType, EnnemyType, getEnnemy } from './models/Ennemy';
import { Terrain, Tile, TileType } from './models/Map';
import { Vector2 } from './models/Utils';

var port: number = 3001;

if (process.env.PORT) {
    port = parseInt(process.env.PORT);
}

interface position {
    x: number;
    y: number;
    rotation: number;
}

interface moveData {
    x: number;
    y: number;
    rotation: number;

    id: string;
}

interface attackData {
    positions: Vector2[];
    id: number;
}

class App {
    private server: http.Server
    private port: number
    private io: socketIO.Server

    private _map: Terrain;
    private _ennemies: {
        [id: string]: Ennemy
    };

    private _users: {
        [socketId: string]: {
            id: string;
            lastPosition: position;
        }
    } = {}

    constructor(port: number) {
        this.port = port

        const app = express()
        //app.use(express.static(path.join(__dirname, '../public')))

        app.get('/', (req, res) => {
            return res.send('Hello world')
        })

        app.get('/users', (req, res) => {
            console.log('retrieving users list')
            return res.json(this._users);
        })

        app.get('/map', (req, res) => {
            console.log('retrieving map')
            return res.json(this._map);
        })

        app.get('/eny', (req, res) => {
            console.log('retrieving ennemies')
            return res.json(Object.values(this._ennemies));
        })

        console.log('path', path.join(__dirname, 'public'))

        this.server = new http.Server(app)
        this.io = new socketIO.Server(this.server, {
            pingInterval: 10000,
            pingTimeout: 5000
        });

        this.io.on('connection', (socket: socketIO.Socket) => {
            console.log('a user connected : ' + socket.id)

            setTimeout(() => {
                let id = uuidv4();

                this._users[socket.id] = {
                    id: id,
                    lastPosition: {
                        x: 0,
                        y: 0,
                        rotation: 0,
                    }
                };

                console.log(`user ${id} created`, this._users);
                socket.emit('connection', id)
                socket.broadcast.emit('remoteconnection', id);
            }, 1000);

            socket.on('move', (d: string) => {
                let data: moveData = JSON.parse(d);
                //console.log('move', data);
                this._users[socket.id].lastPosition.x = data.x;
                this._users[socket.id].lastPosition.y = data.y;
                //socket.emit('hello', {date: new Date().getTime(), data: data});
                socket.broadcast.emit('remotemove', data);
            });

            socket.on('attack', (d: string) => {
                let data: attackData = JSON.parse(d);
                console.log('attack', data);
                Object.values(this._ennemies).forEach(eny => {
                    data.positions.forEach(pos => {
                        //console.log(eny)
                        if (eny.Position.x === pos.x && eny.Position.y === pos.y) {

                            eny.Hit(10);
                            console.log('hit!', eny);
                        }
                    })
                });
                //socket.emit('hello', {date: new Date().getTime(), data: data});
                socket.broadcast.emit('remoteattack', data.id);
            });

            socket.on('disconnect', () => {
                console.log('socket disconnected : ' + socket.id)
                socket.broadcast.emit('remotedisconnect', this._users[socket.id].id);
                delete this._users[socket.id];
                console.log(`user disconnected`, this._users);
            })
        })

        this._map = new Terrain();
        this._ennemies = {};
    }

    public Start() {
        this.server.listen(this.port)
        console.log(`Server listening on port ${this.port}.`)

        let mapData: { [key: string]: Tile } = {}

        for (let x = -10; x <= 10; x++) {
            for (let y = -10; y <= 10; y++) {
                let type = TileType.UNKNOWN;
                if (y === 0 && x === 0) {
                    type = TileType.WATER
                }
                else {
                    type = TileType.GRASS
                }

                mapData[`${x}_${y}`] = {
                    //Players: [],
                    //Ennemies: [],
                    TileType: type
                }
            }
        }

        /*mapData['3_3'].Ennemies.push({
            Id:uuidv4(),
            Name:"Manequin",
            Type:EnnemyType.Dummy
        })*/

        this._map.Load(mapData);

        let eny = getEnnemy(EnnemyType.Dummy, 3, 3, (t, id, a) => {
            console.log('event from eny', t.toString())
            this.io.emit(`ennemy${t.toString()}`, { id, a })
        });

        this._ennemies[eny.Id] = eny;
    }
}

new App(port).Start()