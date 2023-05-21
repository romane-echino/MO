using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public string PlayerId { get; private set; }
    public bool IsLocal { get; private set; }

    [Header("Animation")]
    public float DisplacementLimitToMove = 0.01f;

    [Header("References")]
    [SerializeField]
    private Animator animator;

    private Vector3 lastPosition;
    private bool lastDirAnimationLeft = true;

    public void Init(string id, bool isLocal = false)
    {
        this.PlayerId = id;
        this.IsLocal = isLocal;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        // Animation update
        Vector3 movement = transform.position - lastPosition;
        bool onMovement = Mathf.Abs(movement.x) > DisplacementLimitToMove || Mathf.Abs(movement.y) > DisplacementLimitToMove;
        animator.SetBool("Move", onMovement);

        bool leftDir = movement.x < 0f;
        if (leftDir != lastDirAnimationLeft && onMovement) // Just to be sure to not change the facing when not moving
        {
            animator.SetBool("DirectionLeft", leftDir);
            lastDirAnimationLeft = leftDir;
        }

        lastPosition = transform.position;
    }
}
