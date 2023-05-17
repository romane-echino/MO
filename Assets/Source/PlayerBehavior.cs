using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private float _interval = 0.5f;
    private float _nextTime = 0;

    private PlayerMovement _storedMovement = new PlayerMovement(new Vector3(), "");
    public Vector2 speed = new Vector2(10, 10);

    [Header("Animation")]
    public float DisplacementLimitToMove = 0.1f;

    [Header("References")]
    [SerializeField]
    private Animator animator;

    private bool lastDirAnimationLeft = true;

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

        // Animation update
        bool onMovement = Mathf.Abs(movement.x) > DisplacementLimitToMove || Mathf.Abs(movement.y) > DisplacementLimitToMove;
        animator.SetBool("Move", onMovement);

        bool leftDir = movement.x < 0f;
        if (leftDir != lastDirAnimationLeft && onMovement) // Just to be sure to not change the facing when not moving
        {
            animator.SetBool("DirectionLeft", leftDir);
            lastDirAnimationLeft = leftDir;
        }
    }

    void FixedUpdate()
    {
        if (Time.time >= _nextTime)
        {
            if (this._storedMovement == null)
            {
                this._storedMovement = new PlayerMovement(new Vector3(), "");
            }

            var pos = new Vector3(transform.position.x, transform.position.y, 0);

            if (pos != this._storedMovement.x_y_rotation)
            {
                this._storedMovement.x_y_rotation = pos;
                GameManager.Instance.Network.EmitMovement(new PlayerMovement(pos, "1234"));
            }

            _nextTime += _interval;
        }
    }
}

[System.Serializable]
public class PlayerMovement
{
    public Vector3 x_y_rotation;
    public string id;

    public PlayerMovement(Vector3 x_y_rotation, string id)
    {
        this.x_y_rotation = x_y_rotation;
        this.id = id;
    }
}
