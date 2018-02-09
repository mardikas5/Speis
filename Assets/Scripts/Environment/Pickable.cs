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

    void TryPickUp( Collider other )
    {
        Actor t = other.transform.root.GetComponentInChildren<Actor>();
        if( t != null )
        {
            if( t.transform.root.GetComponentInChildren<Station>().TryDeposit( Data.Resource ) )
            {
                Destroy( transform.root.gameObject );
            }
        }
    }
}
