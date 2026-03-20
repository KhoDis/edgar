using UnityEngine;
using TMPro;

/// <summary>
/// Manages the UI panel shown during item inspection.
/// Wire up via InspectionManager's inspector field.
/// </summary>
public class InspectionUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Tooltip("Close button shown on mobile. Wire its OnClick to OnCloseButtonClicked.")]
    [SerializeField] private GameObject closeButton;

    void Awake()
    {
        Hide();
    }

    public void Show(ItemData data)
    {
        if (titleText != null) titleText.text = data.displayName;
        if (descriptionText != null) descriptionText.text = data.description;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (closeButton != null)
            closeButton.SetActive(true);
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        if (closeButton != null)
            closeButton.SetActive(false);
    }

    /// <summary>
    /// Called by the close button's OnClick event in the Inspector.
    /// </summary>
    public void OnCloseButtonClicked()
    {
        InspectionManager.Instance.CloseInspection();
    }
}