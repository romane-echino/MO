using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class EnnemyManager : MonoBehaviour
{
    public GameObject[] EnnemyPrefabs;

    public Dictionary<string, Transform> Ennemies { get; private set; } = new Dictionary<string, Transform>();
    void Start()
    {
        GameManager.Instance.Network.GetEnnemies();
        ConnectSocketFunctions();
    }

    private void ConnectSocketFunctions()
    {
        GameManager.Instance.Network.socket.On("ennemyhit", (data) =>
        {
            Debug.Log("Remote ennemy hit!" + data);
            var remoteData = data.GetValue<EnnemyHit>();

            Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                Ennemies[remoteData.id].GetComponent<EnnemyBehavior>().Hit(remoteData.a);
            });

        });

        GameManager.Instance.Network.socket.On("ennemydie", (data) =>
        {
            Debug.Log("Remote ennemy die!" + data);
            var remoteData = data.GetValue<EnnemyDie>();
            Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                Ennemies[remoteData.id].GetComponent<EnnemyBehavior>().Die();
            });
        });

        GameManager.Instance.Network.socket.On("ennemyrepop", (data) =>
        {
            Debug.Log("Remote ennemy repop!" + data);
            var remoteData = data.GetValue<EnnemyRepop>();
            Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                Ennemies[remoteData.id].GetComponent<EnnemyBehavior>().Repop();
            });
        });
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetEnnemies(Ennemy[] ennemies)
    {
        var ennemiesRoot = new GameObject("ennemiesRoot");

        foreach (var eny in ennemies)
        {
            var type = System.Enum.Parse<EnnemyType>(eny.Prefab);
            var enyIndex = (int)type;

            var enyGO = Instantiate(this.EnnemyPrefabs[enyIndex], new Vector3(eny.Position.x - .5f, eny.Position.y - .5f, 0), Quaternion.identity, ennemiesRoot.transform);
            enyGO.name = eny.Name;
            enyGO.GetComponent<EnnemyBehavior>().Init(eny);

            this.Ennemies.Add(eny.Id, enyGO.transform);
        }
    }
}


public enum EnnemyType
{
    Dummy = 0
}


[System.Serializable]
public class Ennemy
{
    public string Prefab;
    public string Name;
    public string Id;
    public Vector2 Position;
    public int Life;
    public int MaxLife;
}


//{"id":"dfe36c58-2306-46bf-989f-15e8165913dd","a":90}


/**
ExceptionError converting value "{"id":"dfe36c58-2306-46bf-989f-15e8165913dd","a":80}" to type 'EnnemyHit'. Path '', line 1, position 60.
UnityEngine.Debug:Log (object)
EnnemyManager/<>c:<ConnectSocketFunctions>b__6_0 (SocketIOClient.SocketIOResponse) (at Assets/Source/EnnemyManager.cs:30)
SocketIOClient.SocketIO:EventMessageHandler (SocketIOClient.Messages.EventMessage) (at Library/PackageCache/com.itisnajim.socketiounity@c9e06b1539/Runtime/SocketIOClient/SocketIO.cs:360)
SocketIOClient.SocketIO:OnMessageReceived (SocketIOClient.Messages.IMessage) (at Library/PackageCache/com.itisnajim.socketiounity@c9e06b1539/Runtime/SocketIOClient/SocketIO.cs:454)
System.Reactive.Subjects.Subject`1<SocketIOClient.Messages.IMessage>:OnNext (SocketIOClient.Messages.IMessage)
SocketIOClient.Transport.BaseTransport:OnNext (string) (at Library/PackageCache/com.itisnajim.socketiounity@c9e06b1539/Runtime/SocketIOClient/Transport/BaseTransport.cs:201)
System.Reactive.Subjects.Subject`1<string>:OnNext (string)
SocketIOClient.Transport.SystemNetWebSocketsClientWebSocket/<<Listen>b__14_0>d:MoveNext () (at Library/PackageCache/com.itisnajim.socketiounity@c9e06b1539/Runtime/SocketIOClient/Transport/SystemNetWebSocketsClientWebSocket.cs:86)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()



*/
[System.Serializable]
public class EnnemyHit
{
    public string id;
    public int a;

    public EnnemyHit()
    {

    }
}

[System.Serializable]
public class EnnemyDie
{
    public string id;

    public EnnemyDie()
    {

    }
}

[System.Serializable]
public class EnnemyRepop
{
    public string id;
    public int a;

    public EnnemyRepop()
    {

    }
}