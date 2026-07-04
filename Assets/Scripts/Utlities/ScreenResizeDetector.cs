using UnityEngine;

public class ScreenResizeDetector : MonoBehaviour
{
    private Vector2 lastScreenSize;
    private void Start()
    {
        
        lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            GameEvents.current.ScreenResize();
            lastScreenSize = new Vector2(Screen.width, Screen.height);
        }
    }
}