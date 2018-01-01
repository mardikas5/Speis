using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon : MonoBehaviour
{
    [Serializable]
    public class WeaponData
    {
        public float Damage;
    }

    public WeaponData Data;

    public GameObject WeaponFXPrefab;
    public GameObject WeaponFX;

    public GameObject Projectile;

    public Transform WeaponForward;

    public virtual bool CanFire()
    {
        return true;
    }

    public virtual bool Fire()
    {
        if( !CanFire() )
        {
            return false;
        }
        return true;
    }

    public Collider RayCastHitW( Vector3 start, Vector3 dir, float distance, out RaycastHit t )
    {
        if( Physics.Raycast( start, dir, out t, distance ) )
        {
            return t.collider;
        }
        return null;
    }
}
