using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pickable : MonoBehaviour
{
    [Serializable]
    public class PickableData
    {
        public Storable Resource;
    }

    public PickableData Data;

    public void OnTriggerEnter( Collider other )
    {
        TryPickUp( other );
    }

    public void OnTriggerStay( Collider other )
    {
        TryPickUp( other );
    }

    //Pick up collision layer should be set too.
    void TryPickUp( Collider other )
    {
        //Actor t = other.transform.root.GetComponentInChildren<Actor>();

        if ( other.transform.root.GetComponentInChildren<Structure>().TryDeposit( Data.Resource ) )
        {
            Destroy( transform.root.gameObject );
        }
    }
}

