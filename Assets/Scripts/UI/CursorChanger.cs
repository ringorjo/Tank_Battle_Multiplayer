using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField]
    private Vector2 _cursorSize;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {

        cursorTexture.height = (int)_cursorSize.y;
        cursorTexture.width = (int)_cursorSize.x;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
