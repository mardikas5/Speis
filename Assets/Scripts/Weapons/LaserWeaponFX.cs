using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeaponFX : MonoBehaviour
{
    public Vector3 StartP;
    public Vector3 EndP;


    public float DisappearInterval = .2f;
    public float DisappearTimer = .2f;

    public LineRenderer r;

    public void Use()
    {
        DisappearTimer = DisappearInterval;
        if( r == null )
        {
            r = GetComponent<LineRenderer>();
        }
        r.positionCount = 2;
        r.SetPositions( new Vector3[] { StartP, EndP } );
    }

    public void Start()
    {
        DisappearTimer = DisappearInterval;
    }

    public void Update()
    {
        if( DisappearTimer > 0 )
        {
            DisappearTimer -= Time.deltaTime;
            if( DisappearTimer < 0 )
            {
                r.positionCount = 0;
                //r.SetPositions( new Vector3[] { Vector3.zero, Vector3.zero } );
            }
        }
    }
}
