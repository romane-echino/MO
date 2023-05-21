using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public Texture2D defaultCursor;
    public Texture2D attackCursor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Attack.IsPreparing())
        {
            Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
