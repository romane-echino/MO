using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerBehavior : MonoBehaviour
{

    private CharacterBehavior _character;
    private float _interval = 0.1f;
    private float _nextTime = 0;

    private Vector3 _storedMovement = Vector3.zero;
    public Vector2 speed = new Vector2(10, 10);

    // Start is called before the first frame update
    void Start()
    {
        this._character = GetComponent<CharacterBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this._character.IsLocal)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(speed.x * x, speed.y * y, 0);
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }
    }

    void FixedUpdate()
    {
        if (this._character.IsLocal)
        {
            if (Time.time >= _nextTime)
            {
                if (this._storedMovement == null)
                {
                    this._storedMovement = Vector3.zero;
                }

                var pos = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    0); // todo rotation

                if (pos != this._storedMovement)
                {
                    this._storedMovement = pos;
                    GameManager.Instance.Network.EmitMovement(pos);
                }

                _nextTime += _interval;
            }
        }
    }
}


