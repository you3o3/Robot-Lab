using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;

    public void LoadScene(int sceneBuildIdx)
    {
        StartCoroutine(Load(sceneBuildIdx));
    }

    private IEnumerator Load(int sceneBuildIdx)
    {
        transitionAnimator.SetTrigger("startLevel");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneBuildIdx);
    }
}
