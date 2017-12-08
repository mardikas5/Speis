using System;
using System.Collections.Generic;
using UnityEngine;

//make a gameobject prefab that has all the monos attached.
public class GameSceneInitializer<Singleton> : MonoBehaviour
{
    public MonoBehaviour[] InitializeAtStart = new MonoBehaviour[]
    {

    }
    
    public void Start()
    {
        foreach (MonoBehaviour mb in InitializeAtStart)
        {
            //
        }
    }
    
    public void Init<T>() where T : MonoBehaviour
    {
        gameObject.AddComponent<T>();
    }
}