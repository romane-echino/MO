"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const path_1 = __importDefault(require("path"));
const http_1 = __importDefault(require("http"));
const socket_io_1 = __importDefault(require("socket.io"));
const uuid_1 = require("uuid");
const Ennemy_1 = require("./models/Ennemy");
const Map_1 = require("./models/Map");
var port = 3001;
if (process.env.PORT) {
    port = parseInt(process.env.PORT);
}
class App {
    constructor(port) {
        this._users = {};
        this.port = port;
        const app = (0, express_1.default)();
        app.get('/', (req, res) => {
            return res.send('Hello world');
        });
        app.get('/users', (req, res) => {
            console.log('retrieving users list');
            return res.json(this._users);
        });
        app.get('/map', (req, res) => {
            console.log('retrieving map');
            return res.json(this._map);
        });
        app.get('/eny', (req, res) => {
            console.log('retrieving ennemies');
            return res.json(Object.values(this._ennemies));
        });
        console.log('path', path_1.default.join(__dirname, 'public'));
        this.server = new http_1.default.Server(app);
        this.io = new socket_io_1.default.Server(this.server, {
            pingInterval: 10000,
            pingTimeout: 5000
        });
        this.io.on('connection', (socket) => {
            console.log('a user connected : ' + socket.id);
            setTimeout(() => {
                let id = (0, uuid_1.v4)();
                this._users[socket.id] = {
                    id: id,
                    lastPosition: {
                        x: 0,
                        y: 0,
                        rotation: 0,
                    }
                };
                console.log(`user ${id} created`, this._users);
                socket.emit('connection', id);
                socket.broadcast.emit('remoteconnection', id);
            }, 1000);
            socket.on('move', (d) => {
                let data = JSON.parse(d);
                this._users[socket.id].lastPosition.x = data.x;
                this._users[socket.id].lastPosition.y = data.y;
                socket.broadcast.emit('remotemove', data);
            });
            socket.on('attack', (d) => {
                let data = JSON.parse(d);
                console.log('attack', data);
                Object.values(this._ennemies).forEach(eny => {
                    data.positions.forEach(pos => {
                        if (eny.Position.x === pos.x && eny.Position.y === pos.y) {
                            eny.Hit(10);
                            console.log('hit!', eny);
                        }
                    });
                });
            });
            socket.on('disconnect', () => {
                console.log('socket disconnected : ' + socket.id);
                socket.broadcast.emit('remotedisconnect', this._users[socket.id].id);
                delete this._users[socket.id];
                console.log(`user disconnected`, this._users);
            });
        });
        this._map = new Map_1.Terrain();
        this._ennemies = {};
    }
    Start() {
        this.server.listen(this.port);
        console.log(`Server listening on port ${this.port}.`);
        let mapData = {};
        for (let x = -10; x <= 10; x++) {
            for (let y = -10; y <= 10; y++) {
                let type = Map_1.TileType.UNKNOWN;
                if (y === 0 && x === 0) {
                    type = Map_1.TileType.WATER;
                }
                else {
                    type = Map_1.TileType.GRASS;
                }
                mapData[`${x}_${y}`] = {
                    TileType: type
                };
            }
        }
        this._map.Load(mapData);
        let eny = (0, Ennemy_1.getEnnemy)(Ennemy_1.EnnemyType.Dummy, 3, 3, (t, a) => {
            console.log('event from eny', t.toString());
            this.io.emit(`ennemy${t.toString()}`, a);
        });
        this._ennemies[eny.Id] = eny;
    }
}
new App(port).Start();
