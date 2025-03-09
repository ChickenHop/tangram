using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorIdle;
    [SerializeField] private Texture2D cursorGrab;
    [SerializeField] private AudioClip clickClip;
    public static Cursor instance;
    public CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotspotIdle= Vector2.zero;
    private Vector2 hotspotGrab = new(16, 16);
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        UnityEngine.Cursor.SetCursor(cursorIdle, hotspotIdle, cursorMode);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && clickClip !=null)
        {
            SoundFXManager.instance.playSoundFX(clickClip, transform, 1f);
        }
    }
    public void toGrab()
    {
        UnityEngine.Cursor.SetCursor(cursorGrab, hotspotGrab, cursorMode);
    }

    public void toIdle()
    {
        UnityEngine.Cursor.SetCursor(cursorIdle, hotspotIdle, cursorMode);
    }

    
    
}
