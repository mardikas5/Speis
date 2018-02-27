using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent( typeof( Simulation ) )]
public class GameSceneInitializer : Singleton<GameSceneInitializer>
{
    public bool InitAttachedBehaviours = true;
    public bool InitAllSingletonsInScene = true;
    public bool InitEntitiesInScene = true;

    public bool IsSceneReady = false;

    public event Action SceneLoaded;

    public MonoBehaviour[] InitializeAtStart = new MonoBehaviour[]
    {

    };

    public override void Start()
    {
        base.Start();

        LoadGamedataToScene( new GameData() );
    }

    public void LoadGamedataToScene( GameData data )
    {
        Debug.Log( "Initializing scene for simulation" );

        Time.timeScale = 0;

        IsSceneReady = false;


        if ( InitAllSingletonsInScene )
        {
            LoadSingletons();
        }


        Simulation.Instance.LoadGameData( data );


        if ( InitEntitiesInScene )
        {
            InitAllEntitiesInScene();
        }


        Time.timeScale = 1;

        IsSceneReady = true;

        if ( SceneLoaded != null )
        {
            SceneLoaded();
        }

        Debug.Log( "Scene ready" );
    }

    private void InitAllEntitiesInScene()
    {
        Entity[] allInScene = FindObjectsOfType<Entity>();

        int initCounter = 0;

        foreach ( Entity t in allInScene )
        {
            t.Initialize();

            initCounter++;
        }

        Debug.Log( initCounter + " Entities initialized" );
    }

    private void LoadSingletons()
    {
        List<SingletonBase> list = new List<SingletonBase>( Resources.FindObjectsOfTypeAll<SingletonBase>() );

        foreach ( SingletonBase single in list )
        {
            if ( single == this )
            {
                continue;
            }

            single.Initialize();
        }
    }

    public void Init<T>() where T : MonoBehaviour
    {

    }
}