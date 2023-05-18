using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public string PlayerId { get; private set; }
    public bool IsLocal { get; private set; }

    public void Init(string id, bool isLocal = false)
    {
        this.PlayerId = id;
        this.IsLocal = isLocal;
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
