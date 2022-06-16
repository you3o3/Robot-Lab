using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] icons;
    private int activatedIconIdx;

    public TextMeshProUGUI NameText { get { return nameText; } }
    public TextMeshProUGUI DialogueText { get { return dialogueText; } }

    public void OnButtonClick()
    {
        DialogueManager.Instance.DisplayNextSentence();
    }

    public void ActivateIcon(string name)
    {
        int idx = Array.IndexOf(GeneralInfo.AllNames, name);
        if (idx < 0 || idx > icons.Length)
        {
            Debug.LogError("invalid name or icon not found");
            return;
        }
        icons[idx].SetActive(true);
        activatedIconIdx = idx;
    }

    public void DeactivateIcon()
    {
        icons[activatedIconIdx].SetActive(false);
        activatedIconIdx = 0;
    }
}
