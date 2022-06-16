using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DialogueTrigger : SingletonWMonoBehaviour<DialogueTrigger>
{
    public static DialogueTrigger Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, true);
    }

    [Tooltip("The name of the dialogue should follow \"Level x\" and then a title if GetDialoguesOfLevel() is used.\n" +
        "The name of a hint should be in format \"Level Hint x\"")]
    public Dialogue[] dialogue;

    public void TriggerDialogue(int idx = 0, bool triggerOnlyOnce = true)
    {
        DialogueManager.Instance.StartDialogue(dialogue[idx], triggerOnlyOnce);
    }

    public void TriggerDialogue(Dialogue dialogue, bool triggerOnlyOnce = true)
    {
        DialogueManager.Instance.StartDialogue(dialogue, triggerOnlyOnce);
    }

    public void TriggerScheduledDialogues(Dialogue[] dialogues, bool triggerOnlyOnce = true)
    {
        DialogueManager.Instance.StartScheduledDialogues(dialogues, triggerOnlyOnce);
    }

    public Dialogue[] GetDialoguesOfLevel(int level)
    {
        string levelStr = "Level " + level;
        return dialogue.Where(o => o.Name.Contains(levelStr)).ToArray();
    }

    public Dialogue GetHintOfLevel(int level)
    {
        string hintLevelStr = "Level Hint " + level;
        return dialogue.Where(o => o.Name.Equals(hintLevelStr)).FirstOrDefault();
    }

    public Dialogue GetDialogueByString(string name)
    {
        return dialogue.Where(o => o.Name.Equals(name)).FirstOrDefault();
    }
}
