using System;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent(typeof( Simulation ), typeof( Placement )) ]
public class GameSceneInitializer<Singleton> : MonoBehaviour
{
    public bool InitAttachedBehaviours = true;
    
    public MonoBehaviour[] InitializeAtStart = new MonoBehaviour[]
    {

    }
    
    public void Start()
    {
        //foreach (Component m in GetComponents(typeof())
    }
    
    public void Init<T>() where T : MonoBehaviour
    {
       
    }
}