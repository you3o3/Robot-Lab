using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButtonOnClickAdder : MonoBehaviour
{
    private LevelLoader levelLoader;

    private void Awake()
    {
        levelLoader = GameObject.Find("Scene Loader").GetComponent<LevelLoader>();
        foreach (Transform _child in transform)
        {
            GameObject child = _child.gameObject;
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                string name = child.name;
                int level = int.Parse(name[(name.IndexOf('(') + 1)..(name.Length - 1)]);
                button.onClick.AddListener(delegate { ButtonTask(level); });
                if (level > GeneralInfo.LevelReached)
                    button.interactable = false;
            }
        }
    }

    private void ButtonTask(int level)
    {
        DataBuffer.Instance.level = level;
        levelLoader.LoadScene(GeneralInfo.sceneIdx["CodingScene"]);
        Debug.Log("Level set to " + level);
    }
}
