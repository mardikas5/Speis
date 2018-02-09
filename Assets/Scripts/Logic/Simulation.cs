using System;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : Singleton<Simulation>
{
    [SerializeField]
    public GameData GameData;

    public override void Initialize()
    {
        base.Initialize();
        if( GameData == null )
        {
            GameData = new GameData();
        }
    }

    public void Update()
    {
        if( Input.GetKeyDown( KeyCode.S ) )
        {
            Persistence.SaveGame( "Butts", GameData );
        }
        if( Input.GetKeyDown( KeyCode.L ) )
        {
            GameData = Persistence.LoadGame( "Butts" );
        }


    }

    public void Register( Entity t ) //More generic object
    {
        GameData.Entities.Add( t );
    }

    public void Tick()
    {

    }
}