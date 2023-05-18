using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    private Transform playersTransform;

    private Dictionary<string, PlayerMovement> remotePlayers;

    void Awake()
    {
        var players = new GameObject("Players");
        this.playersTransform = players.transform;

        remotePlayers = new Dictionary<string, PlayerMovement>();
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
        remotePlayers.Add(id, new PlayerMovement(Vector3.zero, id));
    }

    public void MoveRemote(PlayerMovement remoteData)
    {
        //todo remove hack for debug
        if (!remotePlayers.ContainsKey(remoteData.id))
        {
            AddRemotePlayer(remoteData.id);
        }

        remotePlayers[remoteData.id] = remoteData;
    }

    public PlayerMovement GetPlayerPosition(string id)
    {
        return remotePlayers[id];
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
