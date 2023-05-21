using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public Texture2D defaultCursor;
    public Texture2D attackCursor;

    private bool needUpdate = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Attack.IsPreparing())
        {
            needUpdate = true;
            Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
        else
        {
            if (needUpdate)
            {
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
                needUpdate = false;
            }

        }
    }
}
