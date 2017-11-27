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
        
        Connector connect = TryConnect( 5f, Placing  );

        if( connect != null )
        {

        }
        else
        {
            Ray r = placementCam.ScreenPointToRay( Input.mousePosition );
            Placing.transform.position = placementCam.transform.position + ( r.direction.normalized * 15f );
        }
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
            if( ( t.point - hitColliders[i].transform.position ).sqrMagnitude < ( t.point - ClosestCollider.transform.position ).sqrMagnitude )
            {
                ClosestCollider = hitColliders[i];
            }
        }
        
        return ClosestCollider;
    }


    //Tidy all of this shit up real good pls 
    public Connector TryConnect( float distance, GameObject Placing )
    {
        if( placementCam == null )
        {
            return null;
        }
        
        Ray r = placementCam.ScreenPointToRay( Input.mousePosition );
        RaycastHit t;

        if( !Physics.Raycast( r, out t ) )
        {
            return null;
        }

        //get the collider or connector the player is trying to connect to.
        Collider ClosestConnector = GetClosestCollider( t.point, distance, Const.ConnectionLayerMask );

        List<ConnectorEnd> Candidates = Placing.gameObject.GetComponentsInChildren<ConnectorEnd>().ToList();

        ConnectorEnd StationEnd = ClosestConnector.GetComponent<ConnectorEnd>();
        ConnectorEnd PartEnd = null;

        //get the closest connector that is able to connect.
        while( Candidates.Count > 0 )
        {
            if( Candidates.Count > 0 )
            {
                PartEnd = Candidates[0];
            }

            for( int i = 0; i < Candidates.Count; i++ )
            {
                if( ( StationEnd.transform.position - Candidates[i].transform.position ).sqrMagnitude < ( StationEnd.transform.position - PartEnd.transform.position ).sqrMagnitude )
                {
                    PartEnd = Candidates[i];
                }
            }
            
            Placing.transform.eulerAngles = new Vector3( 0, 0, 0 ); //set to whatever rotation the player has currently set 
            
            //check only the part collider.
            Collider checkPlacement = PartEnd.transform.root.GetComponentInChildren<Structure>().PlacementCollider;
            Collider checkOtherPlacement = StationEnd.transform.root.GetComponentInChildren<Structure>().PlacementCollider;
            
            Quaternion Rotation = Quaternion.Inverse( StationEnd.transform.rotation ) * PartEnd.rotation;

            Placing.transform.rotation *= Rotation;
            Placing.transform.position += ( StationEnd.transform.position - PartEnd.transform.position );

            Collider[] inCol = Physics.OverlapBox( checkPlacement.bounds.center, checkPlacement.bounds.extents, Placing.transform.rotation, ~1 << 1, QueryTriggerInteraction.Ignore );
            ExtDebug.DrawBoxCastBox( checkPlacement.bounds.center, checkPlacement.bounds.extents, Placing.transform.rotation, Vector3.zero, 0f, Color.red );

            for( int i = 0; i < inCol.Length; i++ )
            {
                //if collider is inside some object, cant place.
                //if (inCol[i])
            }

            if( inCol.Length > 0 )
            {
                break;
                Candidates.Remove( PartEnd );
            }
            else
            {
                break;
            }
        }
        
        return ClosestConnector.GetComponent<ConnectorEnd>().connector;
    }
}
