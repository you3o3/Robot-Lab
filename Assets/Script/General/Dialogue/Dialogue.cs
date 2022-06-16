using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public string Name;

    // reference: https://answers.unity.com/questions/1687948/disable-background-ui-whenever-a-pop-up-displays.html
    [Header("Dialogue Box")]
    [SerializeField] private GameObject dialogueBoxPanelPrefab;
    public Vector2 position;
    public Vector2 size;

    [Header("Content")]
    [SerializeField] [StringInList(typeof(GeneralInfo), "get_AllNames")] private string name;
    //TODO add change name at page, and change icon?

    public bool IsTriggered { get; set; } = false;

    public GameObject DialogueBox
    {
        get { return dialogueBoxPanelPrefab; }
    }

    public string Speaker
    {
        get { return name; }
        set
        {
            if (RobotInfo.RobotNames.Contains(value))
            {
                name = value;
                DialogueBox.GetComponent<DialogueBox>().NameText.text = value;
            }
            else
            {
                Debug.LogError("Invalid name in Dialogue" + value);
                name = RobotInfo.RobotNames[0];
            }
        }
    }

    [TextArea(3, 10)]
    public string[] sentences;

    public UnityEvent eventAfterDialogue;

}
