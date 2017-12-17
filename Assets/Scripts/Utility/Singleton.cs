using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Singleton<T> : MonoBehaviour where T : class
{
    public virtual void Start()
    {
        if (_instance != null)
         {
             Console.WriteLine("Instance already exists");
             return;
         }
        _instance = this as T;
    }
    
    private static T _instance;

    public static T Instance 
    { 
        get 
        { 
            return _instance;
        }
    }
}