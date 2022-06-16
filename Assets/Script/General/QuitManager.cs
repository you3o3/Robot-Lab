using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuitManager : SingletonWMonoBehaviour<QuitManager>
{
    public static QuitManager Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
        Application.Quit();
    }
}
