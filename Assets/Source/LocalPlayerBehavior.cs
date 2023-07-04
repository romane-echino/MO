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

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        this._character = GetComponent<CharacterBehavior>();

        if (this._character.IsLocal)
        {
            Camera.main.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this._character.IsLocal)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);
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


