using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

// reference: https://www.red-gate.com/simple-talk/development/dotnet-development/saving-game-data-with-unity/
public class SaveManager : SingletonWMonoBehaviour<SaveManager>
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, true);
    }

    private bool fileLoaded = false;

    private void Start()
    {
        if (!fileLoaded)
        {
            LoadFile();
            fileLoaded = true;
        }
    }

    [System.Serializable]
    private class Data
    {
        public int levelReached;
        public Dictionary<int, string> codes;
        public float bgVolume;
        public float seVolume;
    }

    public void SaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/RobotLabData.dat");
        Data data = new();

        data.levelReached = GeneralInfo.LevelReached;
        data.codes = RobotInfo.codes;
        data.bgVolume = BGMusicManager.Instance.GetAllVolume();
        data.seVolume = AudioManager.Instance.totalVolume;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("File saved to " + Application.persistentDataPath + "/RobotLabData.dat");
    }

    public void LoadFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/RobotLabData.dat"))
        {
            Debug.Log("File not exist");
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/RobotLabData.dat", FileMode.Open);
        Data data = (Data)bf.Deserialize(file);
        file.Close();

        GeneralInfo.LevelReached = data.levelReached;
        RobotInfo.codes = data.codes;
        BGMusicManager.Instance.SetAllVolume(data.bgVolume);
        AudioManager.Instance.totalVolume = data.seVolume;

        Debug.Log("File loaded");
    }

    private void OnApplicationQuit()
    {
        SaveFile();
    }
}
