const SignalRJS = require('signalrjs');
const signalR = SignalRJS();

const express = require('express')

//const client = new signalr.Client('http://localhost:8080/signalr', ['testHub'])

const app = express()
app.use(signalR.createListener())
const port = 3000

//api
app.get('/', (req, res) => {
  res.send('Hello World!')
})

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`)
})


//signalr
signalR.on('CONNECTED',function(){
    console.log('connected');
    setInterval(function () {
        signalR.send({time:new Date()});
    },1000)
});

/*
// custom headers
client.headers['Token'] = 'Tds2dsJk'

// set timeout for sending message
client.callTimeout = 10000 // 10's, default 5000

// set delay time for reconecting
client.reconnectDelayTime = 2000 // 2's, default 5000

// set timeout for connect
client.requestTimeout = 2000 // 2's, default 5000


client.on('connected', () => {
    console.log('SignalR client connected.')
})
client.on('reconnecting', (retryCount) => {
    console.log(`SignalR client reconnecting(${retryCount}).`)
})
client.on('disconnected', (reason) => {
    console.log(`SignalR client disconnected(${reason}).`)
})
client.on('error', (error) => {
    console.log(`SignalR client connect error: ${error.code}.`)
})*/