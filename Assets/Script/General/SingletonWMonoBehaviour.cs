using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// static variables will be destroyed on scene load:
// https://answers.unity.com/questions/1169580/static-variable-reset-at-loadlevel.html
// A far more concise version of Singleton:
// https://forum.unity.com/threads/static-variables-and-editor-reload-behavior.615154/

public abstract class SingletonWMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // use it in Awake() to create a singleton instance, if already exists it return null
    protected T CreateInstance(T staticInstance, bool isDontDestoryOnLoad)
    {
        //Debug.Log(staticInstance);
        if (staticInstance != null && staticInstance != this)
        {
            //Debug.Log("Tried to create singleton already exists");
            Destroy(gameObject);
            return staticInstance;
        }

        if (isDontDestoryOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
        return this as T;
    }
}
