using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Health ) )]
public class OnDestroySpawn : MonoBehaviour
{
    public GameObject Spawn;

    void Start()
    {
        GetComponent<Health>().onDestroyed += SpawnItem;
    }

    public void SpawnItem()
    {
        if( Spawn == null )
        {
            return;
        }
        Instantiate( Spawn.gameObject, transform.position, Quaternion.identity, null );
        Destroy( transform.root.gameObject );
    }
}
