using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public Vector2 speed = new Vector2(10, 10);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(speed.x * x, speed.y * y, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);
    }

    void FixedUpdate()
    {
        GameManager.Instance.Network.EmitMovement(new PlayerMovement(
            new Vector2(transform.position.x,transform.position.y),
            0,
            "1234"
        ));
    }
}

[System.Serializable]
    public class PlayerMovement
    {
        public Vector2 position;
        public int rotation;
        public string id;

        public PlayerMovement(Vector2 position, int rotation, string id)
        {
            this.position = position;
            this.rotation = rotation;
            this.id = id;
        }
    }
