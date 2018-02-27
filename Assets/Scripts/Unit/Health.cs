using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Health : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public float MaxHitPoints = 100f;

        public float HitPoints = 100f;

        public float Hardness;
    }

    public Data data;

    public Action onDestroyed;


    public void Start()
    {

    }

    public void GetHit( float Damage )
    {
        data.HitPoints -= Damage;
        if( data.HitPoints <= 0f )
        {
            Dead();
        }
    }

    public void Dead()
    {
        if( onDestroyed != null )
        {
            onDestroyed();
        }
        Destroy( transform.root.gameObject );
    }
}
