using System;
using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    private string URI = "http://localhost:3001";
    //private string URI = "https://mo-server.herokuapp.com";
    public SocketIOUnity socket;
    public double Ping = 0;
    public string playerId;

    // Start is called before the first frame update

    void Awake()
    {
        var uri = new Uri(this.URI);

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
    }

    public void Connect()
    {
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

        socket.On("remotedisconnect", (data) =>
        {
            Debug.Log("Remote Player disconnected");
            Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                GameManager.Instance.Players.RemoveRemotePlayer(data.GetValue<string>());
            });
        });

        socket.On("remotemove", (data) =>
        {
            Debug.Log("Remote move!" + data);
            var remoteData = data.GetValue<PlayerMovement>();

            //Debug.Log("Remote move" + remoteData.id);

            Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                GameManager.Instance.Players.MoveRemote(remoteData);
            });
        });

        socket.On("remoteattack", (data) =>
        {
            Debug.Log("Remote attack!" + data);
            var remoteData = data.GetValue<string>();

            //Debug.Log("Remote move" + remoteData.id);

            Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                GameManager.Instance.Players.AttackRemote(remoteData);
            });
        });


        Debug.Log("Connecting...");
        socket.Connect();
    }

    void Start()
    {



    }

    IEnumerator PostRequest(string uri, string json, Action<string> callback)
    {
        byte[] rawBody = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(rawBody);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("accept", "application/json");
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    yield return webRequest.downloadHandler.text;
                    callback(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator GetRequest(string uri, Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    yield return webRequest.downloadHandler.text;
                    callback(webRequest.downloadHandler.text);
                    break;
            }
        }
    }


    public void GetTerrain()
    {
        StartCoroutine(GetRequest(this.URI + "/map",
        (string json) =>
            {
                var result = JsonConvert.DeserializeObject<Terrain>(json);
                GameManager.Instance.Terrain.SetTerrain(result);
            }));
    }

    public void GetEnnemies()
    {
        StartCoroutine(GetRequest(this.URI + "/eny",
        (string json) =>
            {
                var result = JsonConvert.DeserializeObject<Ennemy[]>(json);
                GameManager.Instance.Ennemies.SetEnnemies(result);
            }));
    }

    public void Authenticate(Authentication auth)
    {

        var json = JsonConvert.SerializeObject(auth);
        StartCoroutine(PostRequest(this.URI + "/login", json,
        (string json) =>
            {
                Debug.Log("json" + json);
                var result = JsonConvert.DeserializeObject<User>(json);
                Debug.Log("res" + result.username);
                GameManager.Instance.Auth.SetLogin(result);
            }));
    }

    public void Register(Authentication auth)
    {
        var json = JsonConvert.SerializeObject(auth);
        StartCoroutine(PostRequest(this.URI + "/register", json,
        (string json) =>
            {
                var result = JsonConvert.DeserializeObject<User>(json);
                GameManager.Instance.Auth.SetLogin(result);
    
            }));
    }

    public void EmitMovement(Vector3 position)
    {
        var json = JsonUtility.ToJson(new PlayerMovement(position, this.playerId));
        //Debug.Log("move!" + json);
        socket.EmitAsync("move", json);
    }

    public void EmitHit(List<Vector2> attacks, string playerId)
    {
        var json = JsonUtility.ToJson(new PlayerAttack(attacks, this.playerId));
        Debug.Log("attack!" + json);
        socket.EmitAsync("attack", json);
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
public class PlayerAttack
{
    public List<Vector2> positions;
    public string id;

    public PlayerAttack(List<Vector2> attacks, string playerId)
    {
        this.positions = attacks;
        this.id = playerId;
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