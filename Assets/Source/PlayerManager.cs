using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    private Transform playersTransform;

    private Dictionary<string, PlayerMovement> remotePlayersData;
    private Dictionary<string, GameObject> remotePlayersGameObjects;

    void Awake()
    {
        var players = new GameObject("Players");
        this.playersTransform = players.transform;

        remotePlayersData = new Dictionary<string, PlayerMovement>();
        remotePlayersGameObjects = new Dictionary<string, GameObject>();
    }

    public void AddLocalPlayer(string id)
    {
        Debug.Log("AddLocalPlayer");
        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, playersTransform);
        player.GetComponent<CharacterBehavior>().Init(id, true);
    }

    public void AddRemotePlayer(string id)
    {
        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, playersTransform);
        player.GetComponent<CharacterBehavior>().Init(id);
        remotePlayersData.Add(id, new PlayerMovement(Vector3.zero, id));
        remotePlayersGameObjects.Add(id, player);
    }

    public void RemoveRemotePlayer(string id)
    {
        if (remotePlayersData.ContainsKey(id))
        {
            remotePlayersData.Remove(id);
        }

        if (remotePlayersGameObjects.ContainsKey(id))
        {
            Destroy(remotePlayersGameObjects[id]);
            remotePlayersGameObjects.Remove(id);
        }
    }

    public void MoveRemote(PlayerMovement remoteData)
    {
        //todo remove hack for debug
        if (!remotePlayersData.ContainsKey(remoteData.id))
        {
            AddRemotePlayer(remoteData.id);
        }

        remotePlayersData[remoteData.id] = remoteData;
    }

    public PlayerMovement GetPlayerPosition(string id)
    {
        return remotePlayersData[id];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
