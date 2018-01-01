using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{
    public override bool Fire()
    {
        if( !base.Fire() )
        {
            return false;
        }

        float dist = 20f;
        RaycastHit hit;

        if( WeaponFX == null )
        {
            WeaponFX = Instantiate( WeaponFXPrefab );
        }

        LaserWeaponFX fX = WeaponFX.GetComponent<LaserWeaponFX>();

        Collider col = RayCastHitW( WeaponForward.position, WeaponForward.transform.forward, dist, out hit );

        fX.StartP = WeaponForward.position;

        if( col != null )
        {
            fX.EndP = hit.point;
            onHitHandler( hit );
        }
        else
        {
            fX.EndP = WeaponForward.transform.position + WeaponForward.transform.forward.normalized * dist;
        }

        fX.Use();

        return true;
    }

    public void onHitHandler( RaycastHit hit )
    {
        Health health = hit.collider.transform.root.GetComponentInChildren<Health>();

        if( health != null )
        {
            health.GetHit( Data.Damage );
        }
    }
}
