using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private NavigationUI navigationUI;
    [SerializeField] private GameObject dialoguePanel;

    public bool IsDialogueActive => dialogueRunner != null && dialogueRunner.IsDialogueRunning;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        if (dialogueRunner != null)
            dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    void OnDestroy()
    {
        if (dialogueRunner != null)
            dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
    }

    public void StartDialogue(string nodeName)
    {
        if (IsDialogueActive) return;
        if (dialogueRunner == null) return;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (InteractionManager.Instance != null) InteractionManager.Instance.enabled = false;
        if (navigationUI != null) navigationUI.enabled = false;

        dialogueRunner.StartDialogue(nodeName);
    }

    private void OnDialogueComplete()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (InteractionManager.Instance != null) InteractionManager.Instance.enabled = true;
        if (navigationUI != null) navigationUI.enabled = true;
    }
}
