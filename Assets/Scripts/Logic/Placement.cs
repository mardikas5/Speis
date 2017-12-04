using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public GameObject testObject;
    
    public GameObject Placing;
    
    public bool placementActive;
    
    public Camera placementCam;
    
    public int PlacementTimer = 5;
    public int PlacementCounter = 0;
    
    public Coroutine placement;
    
    void Start()
    {

    }

    public void SetPlacing(GameObject placing)
    {
        Placing = Instantiate(placing);
        Placing.gameObject.layer = 1 << 1;
        Placing.gameObject.GetComponentsInChildren<Collider>().ToList().ForEach( x => x.gameObject.layer = 1 << 1 );
    }

    void Update()
    {
        if (PlacementCounter < 0)
        {
            PlacementCounter = PlacementTimer;
        }
        else
        {
            PlacementCounter -= 1;
            return;
        }
        if (Placing == null)
        {
            SetPlacing(testObject);
        }
        if (placement == null)
        {
            placement = StartCoroutine(TryConnectRoutine(5f, Placing));
        }
        // else
        // {
        //     Ray r = placementCam.ScreenPointToRay( Input.mousePosition );
        //     Placing.transform.position = placementCam.transform.position + ( r.direction.normalized * 15f );
        // }
    }

    public Collider GetClosestCollider( Vector3 pos, float radius, int layerMask )
    {
        Collider[] hitColliders = Physics.OverlapSphere( pos, radius, layerMask );
        Collider ClosestCollider = null;

        if( hitColliders.Length > 0 )
        {
            ClosestCollider = hitColliders[0];
        }

        for( int i = 0; i < hitColliders.Length; i++ )
        {
            if( ( pos - hitColliders[i].transform.position ).sqrMagnitude < ( pos - ClosestCollider.transform.position ).sqrMagnitude )
            {
                ClosestCollider = hitColliders[i];
            }
        }
        
        return ClosestCollider;
    }
    
    public IEnumerator TryConnectRoutine(float distance, GameObject Placing, Action NextPlacement, Action ConnectorOut)
    {
        //add start rotation
        if( placementCam == null )
        {
            yield return break;
        }
        
        Ray r = placementCam.ScreenPointToRay( Input.mousePosition );
        RaycastHit t;

        if( !Physics.Raycast( r, out t ) )
        {
            yield return break;
        }

        //get the collider or connector the player is trying to connect to.
        Collider ClosestConnector = GetClosestCollider( t.point, distance, Const.ConnectionLayerMask );

        List<ConnectorEnd> Candidates = Placing.gameObject.GetComponentsInChildren<ConnectorEnd>().ToList();

        ConnectorEnd StationEnd = ClosestConnector.GetComponent<ConnectorEnd>();
        ConnectorEnd PartEnd = null;

        //get the closest connector that is able to connect.
        while( Candidates.Count > 0 )
        {
            for( int i = 0; i < Candidates.Count; i++ )
            {
                if( ( StationEnd.transform.position - Candidates[i].transform.position ).sqrMagnitude < ( StationEnd.transform.position - PartEnd.transform.position ).sqrMagnitude )
                {
                    PartEnd = Candidates[i];
                }
            }   
            if (Input.GetMouseButtonDown(0))
            {
                if (TryPlaceTogether( StationEnd, PartEnd, Placing ))
                {
                    Debug.Log( "Fits" );
                }
                else
                {
                    Candidates.Remove( PartEnd );
                }
            }
            yield return null;
        }
        if (Candidates.Count == 0)
        {
            Debug.Log( "Couldn't find suitable candidate" );
        }
        
        return StationEnd.connector;
    }
    
    public bool TryPlaceTogether(ConnectorEnd StationEnd, ConnectorEnd PartEnd, GameObject Placing )
    {
        //check only the part collider.
        Collider checkPlacement = PartEnd.transform.root.GetComponentInChildren<Structure>().PlacementCollider;
        
        Placing.transform.forward -= StationEnd.transform.forward;
        Placing.transform.eulerAngles -= PartEnd.transform.eulerAngles;
        Placing.transform.position += ( StationEnd.transform.position - PartEnd.transform.position );

        List<Collider> inCol = Physics.OverlapBox( checkPlacement.bounds.center, checkPlacement.bounds.extents, Quaternion.identity, ~1 << 1, QueryTriggerInteraction.Ignore ).ToList();
        ExtDebug.DrawBoxCastBox( checkPlacement.bounds.center, checkPlacement.bounds.extents, Quaternion.identity, Vector3.zero, 0f, Color.red );
    
        for( int i = 0; i < inCol.Count; )
        {
            if (inCol[i].transform.root == Placing.transform)
            {   
                inCol.RemoveAt(i);
                continue;
            }
            
            i++;
        }
        if (inCol.Count > 0)
        {
            return false;
        }
            return true;
        }
    }
}
