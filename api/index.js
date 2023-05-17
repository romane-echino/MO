const express = require('express');
const app = express();
const http = require('http');
const server = http.createServer(app);
const socket = require('socket.io');
const { v4: uuidv4 } = require('uuid');


const users = [];

var io = socket(server, {
  pingInterval: 10000,
  pingTimeout: 5000
});

io.use((socket, next) => {
  if (socket.handshake.query.token === "UNITY") {
    next();
  } else {
    next(new Error("Authentication error"));
  }
});



app.get('/', (req, res) => {
  res.sendFile(__dirname + '/index.html');
});

io.on('connection', (socket) => {
  console.log('a user connected');

  setTimeout(() => {
    let id = uuidv4();

    users.push({
      id: id,
      socket: socket.id
    });

    console.log(`user ${id} created`, users);
    socket.emit('connection', id)
  }, 1000);

  socket.on('move', (data) => {
    console.log('move', data);
    //socket.emit('hello', {date: new Date().getTime(), data: data});
    socket.broadcast('move', { date: new Date().getTime(), data: data });
  });
});

server.listen(3000, () => {
  console.log('listening on *:3000');
});