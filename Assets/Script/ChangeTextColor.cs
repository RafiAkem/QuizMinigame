using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText;
    private Color originalColor;
    private Color hoverColor;

    void Start()
    {
        originalColor = buttonText.color;

        if (!ColorUtility.TryParseHtmlString("#E09D00", out hoverColor))
        {
            Debug.LogError("Failed to parse hover color from hex code.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = originalColor;
    }
}
