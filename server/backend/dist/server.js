"use strict";
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
const express_1 = __importDefault(require("express"));
const body_parser_1 = __importDefault(require("body-parser"));
const path_1 = __importDefault(require("path"));
const http_1 = __importDefault(require("http"));
const socket_io_1 = __importDefault(require("socket.io"));
const uuid_1 = require("uuid");
const mongodb_1 = require("mongodb");
const Ennemy_1 = require("./models/Ennemy");
const Map_1 = require("./models/Map");
const Authentication_1 = require("./controllers/Authentication");
var port = 3001;
if (process.env.PORT) {
    port = parseInt(process.env.PORT);
}
class App {
    constructor(port) {
        this._users = {};
        this.port = port;
        this.app = (0, express_1.default)();
        this.app.use(body_parser_1.default.json());
        this.app.use(body_parser_1.default.urlencoded({ extended: false }));
        this.auth = new Authentication_1.Authentication();
        this.app.get('/', (req, res) => {
            return res.send('Hello world');
        });
        this.app.get('/users', (req, res) => {
            console.log('retrieving users list');
            return res.json(this._users);
        });
        this.app.get('/map', (req, res) => {
            console.log('retrieving map');
            return res.json(this._map);
        });
        this.app.get('/eny', (req, res) => {
            console.log('retrieving ennemies');
            return res.json(Object.values(this._ennemies));
        });
        console.log('path', path_1.default.join(__dirname, 'public'));
        this.server = new http_1.default.Server(this.app);
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
                socket.broadcast.emit('remoteattack', data.id);
            });
            socket.on('disconnect', () => {
                console.log('socket disconnected : ' + socket.id);
                socket.broadcast.emit('remotedisconnect', this._users[socket.id].id);
                delete this._users[socket.id];
                console.log(`user disconnected`, this._users);
            });
        });
        let uri = "mongodb+srv://mo-admin:VadeMetro2023;@mo-db.184rodz.mongodb.net/?retryWrites=true&w=majority";
        this.dbClient = new mongodb_1.MongoClient(uri, {
            serverApi: {
                version: mongodb_1.ServerApiVersion.v1,
                strict: true,
                deprecationErrors: true,
            }
        });
        this._map = new Map_1.Terrain();
        this._ennemies = {};
        process.on('exit', this.gracefulShutdown);
        process.on('SIGINT', this.gracefulShutdown);
        process.on('SIGTERM', this.gracefulShutdown);
        process.on('SIGKILL', this.gracefulShutdown);
        process.on('uncaughtException', this.gracefulShutdown);
    }
    Start() {
        return __awaiter(this, void 0, void 0, function* () {
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
            let eny = (0, Ennemy_1.getEnnemy)(Ennemy_1.EnnemyType.Dummy, 3, 3, (t, id, a) => {
                console.log('event from eny', t.toString());
                this.io.emit(`ennemy${t.toString()}`, { id, a });
            });
            this._ennemies[eny.Id] = eny;
            try {
                yield this.dbClient.connect();
                yield this.dbClient.db("admin").command({ ping: 1 });
                this.db = this.dbClient.db('mo-database');
                console.log("Mongo::Init");
                this.auth.Init(this.app, this.db);
            }
            finally {
            }
        });
    }
    gracefulShutdown() {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.dbClient.close();
        });
    }
}
new App(port).Start();
