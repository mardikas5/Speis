using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Health : MonoBehaviour
{
    [Serializable]
    public class HealthData
    {
        public float HitPoints = 100f;

        public float Hardness;
    }

    public HealthData Data;

    public Action onDestroyed;


    public void Start()
    {

    }

    public void GetHit( float Damage )
    {
        Data.HitPoints -= Damage;
        if( Data.HitPoints <= 0f )
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
