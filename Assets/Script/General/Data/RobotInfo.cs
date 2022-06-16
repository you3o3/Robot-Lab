using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInfo : SingletonWMonoBehaviour<RobotInfo>, ISerializationCallbackReceiver
{
    private static RobotInfo instance;

    private void Awake()
    {
        instance = CreateInstance(instance, true);
    }

    // Content
    public static string[] RobotNames { get; } = { "None", "Robo", "Jumpo", "Fireo", "Shorto", "Cheato" };

    public static readonly Dictionary<string, RobotFunction> RobotFunctionDict = new()
    {
        { "None", null },
        { "Robo", new RoboFunc() },
        { "Jumpo", new JumpoFunc() },
        { "Fireo", new FireoFunc() },
        { "Shorto", new ShortoFunc() },
        { "Cheato", new CheatoFunc() }
    };

    public static RobotFunction GetRobotFunctionFromIdx(int robot)
    {
        return RobotFunctionDict[RobotNames[robot]];
    }

    public static Vector2[] MaxJumpDistance { get; } =
    {
        Vector2.zero,
        new Vector2(4.8f, 3.385871f),
        new Vector2(3.96f, 14.40247f),
        new Vector2(2.4f, 3.385871f),
        new Vector2(9.3f, 1.410814f),
        new Vector2(4.8f, 3.385871f)
    };

    public static GameObject[] RobotPrefabs { get; private set; }
    [SerializeField] private GameObject[] helper_RobotPrefabs;

    public static GameObject PlayerBulletPrefab { get; private set; }
    public static GameObject EnemyBulletPrefab { get; private set; }
    [SerializeField] private GameObject helper_PlayerBulletPrefab;
    [SerializeField] private GameObject helper_EnemyBulletPrefab;

    // TODO use this? It is difficult not seeing where player is at, or just set something at 0,0,0 first
    //public static readonly Dictionary<int, Vector3> RobotPosAtLevel = new()
    //{
    //    { 1, new Vector3(0f, 0f, 0f) }
    //};

    // mapping from level to codes
    public static Dictionary<int, string> codes = new();

    public void OnAfterDeserialize()
    {
        RobotPrefabs = helper_RobotPrefabs;
        PlayerBulletPrefab = helper_PlayerBulletPrefab;
        EnemyBulletPrefab = helper_EnemyBulletPrefab;
    }

    public void OnBeforeSerialize()
    {

    }

}
