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
const port = 3000;
class App {
    constructor(port) {
        this._users = {};
        this.port = port;
        const app = (0, express_1.default)();
        app.use(express_1.default.static(path_1.default.join(__dirname, '../public')));
        app.get('/users', (req, res) => {
            console.log('retrieving users list');
            return res.json(this._users);
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
                //console.log('move', data);
                this._users[socket.id].lastPosition.x = data.x;
                this._users[socket.id].lastPosition.y = data.y;
                //socket.emit('hello', {date: new Date().getTime(), data: data});
                socket.broadcast.emit('remotemove', data);
            });
            socket.on('disconnect', () => {
                console.log('socket disconnected : ' + socket.id);
                socket.broadcast.emit('remotedisconnect', { date: new Date().getTime(), data: this._users[socket.id].id });
                delete this._users[socket.id];
                console.log(`user disconnected`, this._users);
            });
        });
    }
    Start() {
        this.server.listen(this.port);
        console.log(`Server listening on port ${this.port}.`);
    }
}
new App(port).Start();
