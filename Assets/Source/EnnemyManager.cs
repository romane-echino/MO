using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyManager : MonoBehaviour
{
    public GameObject[] EnnemyPrefabs;

    public Dictionary<string, Transform> Ennemies { get; private set; } = new Dictionary<string, Transform>();
    void Start()
    {
        GameManager.Instance.Network.GetEnnemies();
        ConnectSocketFunctions();
    }

    private void ConnectSocketFunctions(){
        GameManager.Instance.Network.socket.On("ennemyhit", (data) =>
        {
            Debug.Log("Remote ennemy hit!" + data);
            
        });

        GameManager.Instance.Network.socket.On("ennemydie", (data) =>
        {
            Debug.Log("Remote ennemy die!" + data);
        });

        GameManager.Instance.Network.socket.On("ennemyrepop", (data) =>
        {
            Debug.Log("Remote ennemy repop!" + data);
            //Debug.Log("Remote move!" + data);
            //var remoteData = data.GetValue<PlayerMovement>();

            //Debug.Log("Remote move" + remoteData.id);

            /*Dispatcher.UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                GameManager.Instance.Players.MoveRemote(remoteData);
            });*/
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
}