using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NavigationUI : MonoBehaviour
{
    [SerializeField] private CameraNavigationSystem navSystem;
    [SerializeField] private Transform buttonContainer;  // HorizontalLayoutGroup parent
    [SerializeField] private Button buttonPrefab;

    void Start()
    {
        navSystem.OnCameraChanged += _ => RebuildButtons();
    }

    void RebuildButtons()
    {
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        foreach (var neighborId in navSystem.GetNeighbors())
        {
            string id = neighborId; // capture for lambda
            var btn = Instantiate(buttonPrefab, buttonContainer);
            var label = btn.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = navSystem.GetDisplayName(id);
            btn.onClick.AddListener(() => navSystem.GoTo(id));
        }
    }
}
