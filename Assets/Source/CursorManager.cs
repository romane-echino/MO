using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public Texture2D defaultCursor;
    public Texture2D attackCursor;

    private bool needUpdate = true;

    public CursorType actualCursor = CursorType.None;

    private void Update()
    {
        CursorType newCursor = CursorType.None;
        if (GameManager.Instance.Attack.IsPreparing())
        {
            newCursor = CursorType.Attack;
        }
        else
        {
            newCursor = CursorType.Default;
        }

        if(newCursor != actualCursor)
        {
            switch (newCursor)
            {
                case CursorType.None:
                    break;
                case CursorType.Default:
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.Attack:
                    Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    break;
            }
            actualCursor = newCursor;
        }
    }

    public enum CursorType
    {

        None = 0,
        Default = 1,
        Attack = 2,
    }
}
