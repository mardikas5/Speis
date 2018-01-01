using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mineable : Entity
{

    [System.Serializable]
    public class MineableData
    {
        public Resource[] Resources;

        public GameObject SpawnOnMined;
    }


    public MineableData Data;

    public override bool Initialize()
    {
        base.Initialize();

        GetComponent<Health>().onDestroyed += OnDestroyHandler;

        return true;
    }

    void OnDestroyHandler()
    {
        foreach( Resource r in Data.Resources )
        {
            GameObject MinedPiece = Instantiate( Data.SpawnOnMined, transform.position, Quaternion.identity, null );
            MinedPiece.GetComponent<Pickable>().Data.Resource = r;
        }
    }
}
