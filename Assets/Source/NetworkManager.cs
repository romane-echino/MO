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
    public string playerId;

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


        socket.On("connection", (data) =>
        {
            Debug.Log("Connection success");
            Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                GameManager.Instance.Players.AddLocalPlayer(data.GetValue<string>());
            });

            this.playerId = data.GetValue<string>();
        });

        socket.On("remoteconnection", (data) =>
        {
            Debug.Log("Remote Player connected");
            Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                GameManager.Instance.Players.AddRemotePlayer(data.GetValue<string>());
            });
        });

        socket.On("remotemove", (data) =>
        {
            Debug.Log("Remote move!" + data);
            var remoteData = data.GetValue<RemotePlayerMovement>();

            Debug.Log("Remote move" + remoteData.data.id);
        });


        Debug.Log("Connecting...");
        socket.Connect();
    }

    public void EmitMovement(Vector3 position)
    {
        var json = JsonUtility.ToJson(new PlayerMovement(position, this.playerId));
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

[System.Serializable]
public class PlayerMovement
{
    public int x;
    public int y;
    public int rotation;
    public string id;

    public PlayerMovement(Vector3 x_y_rotation, string id)
    {
        this.x = Mathf.FloorToInt(x_y_rotation.x * 100f);
        this.y = Mathf.FloorToInt(x_y_rotation.y * 100f);
        this.rotation = Mathf.FloorToInt(x_y_rotation.z * 100f);

        this.id = id;
    }
}



[System.Serializable]
public class RemotePlayerMovement
{
    public DateTime date;
    public PlayerMovement data;

    public RemotePlayerMovement(DateTime date, PlayerMovement data)
    {
        this.date = date;
        this.data = data;
    }
}