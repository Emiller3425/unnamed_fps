using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Crosshairs : MonoBehaviour
{
    public Image top;
    public Image bottom;
    public Image left;
    public Image right;
    private RectTransform crossHairRectTransform;
    public float bloomSize;
    private float targetOffset;
    private float maxBloomFromShotReset = 0.05f;
    private float currentBloomFromShotReset;

    public float currentBloomRadius
    {
        get
        {
            return (top.rectTransform.anchoredPosition.y - bottom.rectTransform.anchoredPosition.y) / 2;
        }
    }
    private void Awake()
    {
        crossHairRectTransform = GetComponent<RectTransform>();
        crossHairRectTransform.anchoredPosition = Vector2.zero;
        GameEvents.current.OnSetCrossHairActivated += SetCrossHairActivated;
        GameEvents.current.OnSetCrossHairDeactivated += SetCrossHairDeactivated;
        GameEvents.current.OnBloom += HandleCrossHairBloom;
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

        bloomSize = absoluteValueTargetOffset * 3f;
        
        // Decriments the bloom timer 
        if (currentBloomFromShotReset > 0f)
        {
            currentBloomFromShotReset -= Time.deltaTime;
        }
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

    public void HandleCrossHairBloom(float velocity, bool fromShotBullet)
    {
        if (fromShotBullet)
        {
            targetOffset = velocity;
            currentBloomFromShotReset = maxBloomFromShotReset;
        }
        else if (currentBloomFromShotReset  <= 0f)
        {
            targetOffset = velocity;
        }
    }

    private void OnDestoy()
    {
        GameEvents.current.OnSetCrossHairActivated -= SetCrossHairActivated;
        GameEvents.current.OnSetCrossHairDeactivated -= SetCrossHairDeactivated;
    }

}