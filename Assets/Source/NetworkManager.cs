using System;
using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public SocketIOUnity socket;
    public double Ping = 0;

    // Start is called before the first frame update
    void Start()
    {
        var uri = new Uri("http://192.168.1.234:3000");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
            ,
            EIO = 4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        ///// reserved socketio events
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("socket.OnConnected");
        };
        socket.OnPing += (sender, e) =>
        {
        };
        socket.OnPong += (sender, e) =>
        {
            this.Ping = e.TotalMilliseconds;
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("disconnect: " + e);
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Log($"{DateTime.Now} Reconnecting: attempt = {e}");
        };


        socket.On("connection", (data) => {
            Debug.Log(data.ToString());
        });


        Debug.Log("Connecting...");
        socket.Connect();
    }

    public void EmitMovement(PlayerMovement movement)
    {
        var json = JsonUtility.ToJson(movement);
        Debug.Log("move!" + json);
        socket.EmitAsync("move", json);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnApplicationQuit()
    {
        socket.Disconnect();
    }
}
