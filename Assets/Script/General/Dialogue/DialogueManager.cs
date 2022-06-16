using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DialogueManager : SingletonWMonoBehaviour<DialogueManager>
{
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, true);
    }

    //TODO add dialogue manager and dialogue trigger to hierarchy
    // dialogue box prefeb created, fine tune it
    // if scale not appropriate, see https://forum.unity.com/threads/how-to-scale-child-like-it-was-part-of-parent.775421/

    //Content
    private GameObject UI;
    private Queue<string> sentences;
    private UnityEvent eventAfterDialogue;

    private Coroutine currCoroutine;
    private GameObject instantiatedDialogueBox, instantiatedDialogueBoxPanel;
    private TextMeshProUGUI dialogueText;

    private bool isScheduled = false, scheduleTriggerOnlyOnce = true;
    private Queue<Dialogue> scheduledDialogues = new();

    private void Start()
    {
        sentences = new();
        // see https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-activeSceneChanged.html
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    public void Init()
    {
        //FIXME this is not a good way to set the UI.
        UI = GameObject.Find("UI Manager");
    }

    private void Update()
    {
        if (!isScheduled) return;
        if (scheduledDialogues.Count == 0) // no more dialogues
        {
            isScheduled = false;
            scheduleTriggerOnlyOnce = true;
            return;
        }
        if (instantiatedDialogueBox != null) return; // current dialogue not end

        StartDialogue(scheduledDialogues.Dequeue(), scheduleTriggerOnlyOnce);
    }

    public void StartDialogue(Dialogue dialogue, bool triggerOnlyOnce = true)
    {
        // TODO here I used is triggered to track dialogues that are already presented and don't present again
        // maybe allow player to revisit the dialogues somewhere
        if (triggerOnlyOnce)
        {
            if (dialogue.IsTriggered) return;
            dialogue.IsTriggered = true;
        }
        
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        eventAfterDialogue = dialogue.eventAfterDialogue;

        instantiatedDialogueBoxPanel = Instantiate(dialogue.DialogueBox, UI.transform.position, Quaternion.identity, UI.transform);
        instantiatedDialogueBox = instantiatedDialogueBoxPanel.transform.GetChild(0).gameObject;
        // box position and size
        RectTransform rt = instantiatedDialogueBox.GetComponent<RectTransform>();
        rt.anchoredPosition = dialogue.position;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dialogue.size.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dialogue.size.y);

        // box content
        DialogueBox db = instantiatedDialogueBox.GetComponent<DialogueBox>();
        dialogueText = db.DialogueText;
        db.NameText.text = dialogue.Speaker;
        db.ActivateIcon(dialogue.Speaker);

        DisplayNextSentence();
    }

    public void StartScheduledDialogues(Dialogue[] dialogues, bool triggerOnlyOnce = true)
    {
        isScheduled = true;
        scheduleTriggerOnlyOnce = triggerOnlyOnce;
        scheduledDialogues = new Queue<Dialogue>(dialogues);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (currCoroutine != null) StopCoroutine(currCoroutine);
        currCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        currCoroutine = null;
    }

    private void EndDialogue()
    {
        Destroy(instantiatedDialogueBox);
        Destroy(instantiatedDialogueBoxPanel);
        instantiatedDialogueBox = null;
        instantiatedDialogueBoxPanel = null;

        if (eventAfterDialogue != null)
        {
            eventAfterDialogue.Invoke();
            eventAfterDialogue = null;
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        // dispose all dialogue
        StopAllCoroutines();
        sentences = new();
        eventAfterDialogue = null;
        currCoroutine = null;

        instantiatedDialogueBox = null;
        instantiatedDialogueBoxPanel = null;

        isScheduled = false;
        scheduleTriggerOnlyOnce = true;
        scheduledDialogues = new();
    }
}
