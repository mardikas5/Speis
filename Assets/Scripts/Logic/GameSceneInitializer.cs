using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Simulation ) )]
public class GameSceneInitializer : Singleton<GameSceneInitializer>
{
    public bool InitAttachedBehaviours = true;
    public bool InitAllSingletonsInScene = true;

    public MonoBehaviour[] InitializeAtStart = new MonoBehaviour[]
    {

    };

    public override void Start()
    {
        if( InitAllSingletonsInScene )
        {
            LoadSingletons();
        }
    }

    private void LoadSingletons()
    {
        List<SingletonBase> list = new List<SingletonBase>( Resources.FindObjectsOfTypeAll<SingletonBase>() );
        foreach( SingletonBase single in list )
        {
            if( single == this )
            {
                continue;
            }
            single.Initialize();
        }
        //foreach( SingletonBase single in list )
        //{
        //    if( single == this )
        //    {
        //        continue;
        //    }
        //    single.Start();
        //}
    }

    public void Init<T>() where T : MonoBehaviour
    {

    }
}