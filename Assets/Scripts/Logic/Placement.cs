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

    public bool stop = false;
    public float stopcounter = 200;
    public float stopin = 200;
    void Start()
    {

    }

    void Update()
    {
        if( stop )
        {
            stopin -= 1;
            if( stopin < 0 )
            {
                stop = false;
                stopin = stopcounter;
            }
            return;
        }
        if( Placing == null )
        {
            Placing = Instantiate( testObject );
            Placing.gameObject.layer = 1 << 1;
            Placing.gameObject.GetComponentsInChildren<Collider>().ToList().ForEach( x => x.gameObject.layer = 1 << 1 );
        }

        Vector3 pos = Vector3.zero;
        Vector3 rot = Vector3.zero;
        Connector connect = TryConnect( 5f, out pos, out rot );

        if( connect != null )
        {
            //Placing.transform.forward = rot;
            //Placing.transform.position = pos;
        }
        else
        {
            Ray r = placementCam.ScreenPointToRay( Input.mousePosition );
            Placing.transform.position = placementCam.transform.position + ( r.direction.normalized * 15f );
        }
    }

    //Tidy all of this shit up real good pls 
    public Connector TryConnect( float distance, out Vector3 pos, out Vector3 rot )
    {
        pos = Vector3.zero;
        rot = Vector3.zero;
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

        Collider[] hitColliders = Physics.OverlapSphere( t.point, distance, Const.ConnectionLayerMask );
        Collider ClosestConnector = null;



        if( hitColliders.Length > 0 )
        {
            ClosestConnector = hitColliders[0];
        }

        for( int i = 0; i < hitColliders.Length; i++ )
        {
            if( ( t.point - hitColliders[i].transform.position ).sqrMagnitude < ( t.point - ClosestConnector.transform.position ).sqrMagnitude )
            {
                ClosestConnector = hitColliders[i];
            }
        }

        List<ConnectorEnd> ends = Placing.gameObject.GetComponentsInChildren<ConnectorEnd>().ToList();
        //placed connector (already on station)
        ConnectorEnd ConnectWith = ClosestConnector.GetComponent<ConnectorEnd>();
        ConnectorEnd ConnectEnd = null;

        while( ends.Count > 0 )
        {

            if( ends.Count > 0 )
            {
                ConnectEnd = ends[0];
            }

            for( int i = 0; i < ends.Count; i++ )
            {
                if( ( ConnectWith.transform.position - ends[i].transform.position ).sqrMagnitude < ( ConnectWith.transform.position - ConnectEnd.transform.position ).sqrMagnitude )
                {
                    ConnectEnd = ends[i];
                }
            }
            Placing.transform.eulerAngles = new Vector3( 0, 0, 0 );

            Collider checkPlacement = ConnectEnd.transform.root.GetComponentInChildren<Structure>().PlacementCollider;
            Collider checkOtherPlacement = ConnectWith.transform.root.GetComponentInChildren<Structure>().PlacementCollider;
            Vector3 diff = -( ConnectEnd.transform.forward + ( ConnectWith.transform.forward - ConnectEnd.transform.forward ) );

            Debug.Log( diff + ", " + ConnectEnd.transform.forward + ", " + ConnectWith.transform.forward + ", " + ( ConnectWith.transform.forward - ConnectEnd.transform.forward ) );
            Placing.transform.forward = -ConnectWith.transform.forward;
            Placing.transform.forward +=  ConnectEnd.transform.forward;
            Placing.transform.position += ( ConnectWith.transform.position - ConnectEnd.transform.position );

            Collider[] inCol = Physics.OverlapBox( checkPlacement.bounds.center, checkPlacement.bounds.extents, Placing.transform.rotation, ~1 << 1, QueryTriggerInteraction.Ignore );
            ExtDebug.DrawBoxCastBox( checkPlacement.bounds.center, checkPlacement.bounds.extents, Placing.transform.rotation, Vector3.zero, 0f, Color.red );
            //GetComponent<LineRenderer>().SetPositions( new Vector3[] { ConnectWith.transform.position, ConnectEnd.transform.position } );
            //Placing.transform.rotation = Placing.transform.rotation;
            for( int i = 0; i < inCol.Length; i++ )
            {
                //if (inCol[i])
            }

            if( inCol.Length > 0 )
            {
                break;
                ends.Remove( ConnectEnd );
            }
            else
            {
                break;
            }
        }
        stop = true;
        return ClosestConnector.GetComponent<ConnectorEnd>().connector;
    }


}
