using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteraction : MonoBehaviour
{
    [Header("Fade out")]
    [SerializeField] private float deathFadeOutTime = 1;
    private float fadeOutGap;
    private SpriteRenderer srend;
    private bool isAlive;

    [Header("Deactivate")]
    [SerializeField] private Behaviour[] components;

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

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void EnemyDie()
    {
        if (!isAlive) return;
        Debug.Log("Enemy die");
        Deactivate();
        Fading.Instance.StartFadeOut(srend, deathFadeOutTime, () => { gameObject.SetActive(false); });
    }

    public void Deactivate()
    {
        isAlive = false;
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }
}
