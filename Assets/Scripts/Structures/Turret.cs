using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Structure
{

    [Tooltip( "Turn rate of the turret in degrees per second" )]
    public float TurnRate = 1f;

    public Entity Target;
    public Vector3 Targetv3;

    public Weapon Weapon;

    // Use this for initialization
    void Start()
    {
        Weapon = GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetAndShoot();
    }

    void TargetAndShoot()
    {
        if( Target == null )
        {
            return;
        }

        Vector3 targetDir = Target.transform.position - transform.position;
        float step = TurnRate * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards( Weapon.WeaponForward.transform.forward, targetDir, step, 0.0F );
        Debug.DrawRay( transform.position, newDir, Color.red );
        Weapon.WeaponForward.transform.rotation = Quaternion.LookRotation( newDir );

        if( ( Weapon.WeaponForward.forward - targetDir.normalized ).magnitude < .05f )
        {
            Weapon.Fire();
        }
        //Debug.Log(transform)
    }
}
