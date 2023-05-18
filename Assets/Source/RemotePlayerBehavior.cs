using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerBehavior : MonoBehaviour
{
    private CharacterBehavior _character;

    public float animationSpeed = .5f;

    // Start is called before the first frame update
    void Start()
    {
        this._character = GetComponent<CharacterBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this._character.IsLocal)
        {
            var data = GameManager.Instance.Players.GetPlayerPosition(this._character.PlayerId);
            var target = new Vector3(data.x / 100f, data.y / 100f, 0);
            transform.position = Vector3.Lerp(transform.position, target, Time.smoothDeltaTime * animationSpeed);
        }
    }
}
