using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class Mineable : Entity
{
    public Storable[] Resources;

    public GameObject SpawnOnMined;

    public override bool Initialize()
    {
        base.Initialize();

        GetComponent<Health>().onDestroyed += OnDestroyHandler;

        return true;
    }

    void OnDestroyHandler()
    {
        foreach( Storable r in Resources )
        {
            GameObject MinedPiece = Instantiate( SpawnOnMined, transform.position, Quaternion.identity, null );
            MinedPiece.GetComponent<Pickable>().Data.Resource = r;
        }
    }
}
