using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// reference: https://answers.unity.com/questions/1580211/when-an-object-is-dontdestroyonload-where-can-i-pu.html
public class Initializer : MonoBehaviour
{
    [SerializeField] private UnityEvent initializerFunctions;

    void Start()
    {
        initializerFunctions.Invoke();
        if (DialogueManager.Instance != null) DialogueManager.Instance.Init();
        Destroy(gameObject);
    }

}
