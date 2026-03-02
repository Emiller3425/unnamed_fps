using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HitMarker : MonoBehaviour
{
    private RectTransform hitMarkerRectTransform;
    void Awake()
    {
        hitMarkerRectTransform = GetComponent<RectTransform>();
        hitMarkerRectTransform.anchoredPosition = Vector2.zero;
        GameEvents.current.OnSetHitMarkerActivated += SetHitMarkerActivated;
        GameEvents.current.OnSetHitMarkerDeactivated += SetHitMarkerDeactivated;
    }

    void Start()
    {
        SetHitMarkerDeactivated();
    }

    public async void SetHitMarkerActivated()
    {
        gameObject.SetActive(true);
        await Task.Delay(75);
        SetHitMarkerDeactivated();
    }

    public void SetHitMarkerDeactivated()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnSetHitMarkerActivated -= SetHitMarkerActivated;
        GameEvents.current.OnSetHitMarkerDeactivated -= SetHitMarkerDeactivated;
    }

}