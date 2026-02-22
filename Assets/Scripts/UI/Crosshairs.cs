
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

// TODO: make cross hair widen when walking, and after shot. instaed of hidding middle, should be within crosshair bounds

[RequireComponent(typeof(RectTransform))]
public class Crosshairs : MonoBehaviour
{
    public Image top;
    public Image bottom;
    public Image left;
    public Image right;
    private RectTransform crossHairRectTransform;
    private float targetOffset;
    private void Awake()
    {
        crossHairRectTransform = GetComponent<RectTransform>();
        crossHairRectTransform.anchoredPosition = Vector2.zero;
        GameEvents.current.OnSetCrossHairActivated += SetCrossHairActivated;
        GameEvents.current.OnSetCrossHairDeactivated += SetCrossHairDeactivated;
        GameEvents.current.OnPlayerVelocityChanged += HandleCrossHairBloom;
    }

    private void OnEnable()
    {
            top.rectTransform.anchoredPosition = new Vector2(0f, 3f);
            bottom.rectTransform.anchoredPosition = new Vector2(0f, -3f);
            left.rectTransform.anchoredPosition = new Vector2(-3f, 0f);
            right.rectTransform.anchoredPosition = new Vector2(3f, 0f);
    }

    private void Update()
    {
        float absoluteValueTargetOffset = Mathf.Abs(targetOffset);

        // calculations
        top.rectTransform.anchoredPosition = Vector2.Lerp(top.rectTransform.anchoredPosition, new Vector2(0f, 3f * absoluteValueTargetOffset), 0.075f);
        bottom.rectTransform.anchoredPosition = Vector2.Lerp(bottom.rectTransform.anchoredPosition, new Vector2(0f, -3f * absoluteValueTargetOffset), 0.075f);
        left.rectTransform.anchoredPosition = Vector2.Lerp(left.rectTransform.anchoredPosition, new Vector2(-3f * absoluteValueTargetOffset, 0f), 0.075f);
        right.rectTransform.anchoredPosition = Vector2.Lerp(right.rectTransform.anchoredPosition, new Vector2(3f * absoluteValueTargetOffset, 0f), 0.075f);
    }

    public void SetCrossHairActivated()
    {
        gameObject.SetActive(true);
    }

    public void SetCrossHairDeactivated()
    {
        gameObject.SetActive(false);
    }

    public void SetCrossHairPosition(Vector2 newPosition)
    {
        crossHairRectTransform.anchoredPosition = newPosition;
    }

    public void HandleCrossHairBloom(float velocity)
    {
        targetOffset = velocity;
    }

    private void OnDestoy()
    {
        GameEvents.current.OnSetCrossHairActivated -= SetCrossHairActivated;
        GameEvents.current.OnSetCrossHairDeactivated -= SetCrossHairDeactivated;
    }

}