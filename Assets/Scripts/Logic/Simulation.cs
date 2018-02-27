using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Simulation : Singleton<Simulation>
{
    [SerializeField]
    private GameData GameData;

    public override void Initialize()
    {
        base.Initialize();

        if ( GameData == null )
        {
            GameData = new GameData();
        }
    }


    public void Update()
    {
        if ( Input.GetKeyDown( KeyCode.S ) )
        {
            foreach ( ProtoBase p in GameData.ProtoBaseObjects)
            {
                p.Save();
            }

            Persistence.SaveGame( "Butts", GameData );
        }
        if ( Input.GetKeyDown( KeyCode.L ) )
        {
            BootManager.Instance.LoadGame( Persistence.LoadGame( "Butts" ) );
        }
    }


    public void LoadGameData( GameData data )
    {
        if ( data == null )
        {
            //new game?
        }

        GameData = data;

        data.LoadIntoScene();
    }


    public T FindEntityInGameData<T>( string Guid ) where T : ProtoBase
    {
        ProtoBase t = GameData.ProtoBaseObjects.Find( ( x ) => x.instanceGUID == Guid );

        return t as T;
    }

    public T FindEntityInScene<T>( string Guid ) where T : Entity
    {
        return FindObjectsOfType<T>().Where( ( x ) => x.instanceGUID == Guid ).FirstOrDefault();
    }


    public void Register( ProtoBase p )
    {
        if ( p == null )
        {
            return;
        }

        if ( GameData.ProtoBaseObjects.Find( ( x ) => x.instanceGUID == p.instanceGUID ) != null )
        {
            return;
        }

        GameData.ProtoBaseObjects.Add( p );
    }


    public void Tick()
    {
        Debug.Log( "Ticking" );
    }
}