using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Fade out")]
    [SerializeField] private float deathFadeOutTime = 1;
    private float fadeOutGap;
    private SpriteRenderer srend;
    private bool isAlive;

    [Header("Deactivate")]
    [SerializeField] private Behaviour[] components;

    public bool PlayerDisabled { private set; get; } = false;

    private void Awake()
    {
        srend = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeOutGap = deathFadeOutTime / 20;
        isAlive = true;
    }

    public void PlayerDie()
    {
        if (!isAlive) return;
        if (RobotData.Instance.IsTestRun) return; // if test run then invincible
        Debug.Log("Player hit by enemy");
        Deactivate();
        Fading.Instance.StartFadeOut(srend, deathFadeOutTime, () => { gameObject.SetActive(false); });
        GameManager.Instance.PlayerDied();
    }

    public void PlayerWin()
    {
        Debug.Log("Player win");
        Deactivate();
        GameManager.Instance.PlayerWon();
    }

    // only disable player manual control on robot
    public void DisablePlayer()
    {
        PlayerDisabled = true;
    }

    public void EnablePlayer()
    {
        PlayerDisabled = false;
    }

    private void Deactivate()
    {
        isAlive = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }
}
