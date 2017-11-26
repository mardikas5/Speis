using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{	
    void Start()
    {
        
    }

    void Update()
    {

    }

    public Connector TryConnect(float distance)
    {
        Ray r = Camera.current.ScreenPointToRay(Input.mousePosition);
        RaycastHit t;

        if (!Physics.Raycast(r, out t))
        {
            return null;
        }

        Collider[] hitColliders = Physics.OverlapSphere(t.point, distance, Const.ConnectionLayerMask);
        Collider close = null;

        if (hitColliders.Length > 0)
        {
            close = hitColliders[0];
        }

        for (int i = 0; i < hitColliders.Length;i++)
        {
            if ((t.point - hitColliders[i].transform.position).sqrMagnitude < (t.point - close.transform.position).sqrMagnitude)
            {
                close = hitColliders[i];
            }
        }
        Debug.Log(close.gameObject.name);
        return close.GetComponent<Connector>();
    }
}
