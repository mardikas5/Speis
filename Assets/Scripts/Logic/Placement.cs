using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//namespace Structures
public class Placement<Singleton> : MonoBehaviour
{
    [Serializable]
    public class InputHandler
    {
        public KeyCode startPlacementKey =      KeyCode.R;
        public KeyCode swapPortKey =            KeyCode.H;
        public KeyCode finalizeConnectionKey =  KeyCode.J;
        
        public Func<bool> startPlacement = isKeyDown( startPlacementKey );
        public Func<bool> swapPort = isKeyDown( swapPortKey );
        public Func<bool> finalizeConnection = isKeyDown( finalizeConnectionKey );
        
        public bool isKeyDown( KeyCode k)
        {
            return Input.GetKeyDown(k);
        }
    }
    
    [SerializeField]
    public InputsHandler inputsHandler;
    
    public GameObject testObject;
    public GameObject Placing;
    
    public bool placementActive;

    public Camera placementCam;

    public Coroutine PlacementCoroutine;

    public event Action<GameObject> BuildingPlaced;
    
    public void SetPlacing( GameObject placing )
    {
        Placing = Instantiate( placing );
        Placing.gameObject.layer = 1 << 1;
        Placing.gameObject.GetComponentsInChildren<Collider>().ToList().ForEach( x => x.gameObject.layer = 1 << 1 );
    }


    void Update()
    {
        UpdatePlacement();
    }
    
    
    public void UpdatePlacement()
    {
        if ( inputsHandler.startPlacement() )
        {
            if( Placing == null )
            {
                SetPlacing( testObject );
            }
        }
        if( PlacementCoroutine == null && Placing != null )
        {
            PlacementCoroutine = StartCoroutine( TryConnectRoutine( 5f, Placing ) );
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
            if( ( pos - hitColliders[i].transform.position ).sqrMagnitude < ( pos - ClosestCollider.transform.position ).sqrMagnitude )
            {
                ClosestCollider = hitColliders[i];
            }
        }

        return ClosestCollider;
    }


    public ConnectorEnd GetConnectorAtScreenPoint( Vector2 Pos, float distance )
    {
        if( placementCam == null )
        {
            return null;
        }

        Ray r = placementCam.ScreenPointToRay( Pos );
        RaycastHit t;

        if( !Physics.Raycast( r, out t ) )
        {
            return null;
        }

        Collider ClosestConnector = GetClosestCollider( t.point, distance, Const.ConnectionLayerMask );
        //get the collider or connector the player is trying to connect to.

        if( ClosestConnector == null )
        {
            return null;
        }

        return ClosestConnector.GetComponent<ConnectorEnd>();
    }


    //can add callback to some value
    public IEnumerator TryConnectRoutine( float distance, GameObject Placing )
    {
        ConnectorEnd ConnectToStation = null;
        ConnectorEnd PartEnd = null;
        ConnectorEnd StationEnd = GetConnectorAtScreenPoint( Input.mousePosition, distance );
        

        if( StationEnd == null )
        {
            yield break;
        }

        List<ConnectorEnd> Candidates = Placing.transform.root.GetComponentsInChildren<ConnectorEnd>().ToList();

        PartEnd = Candidates[0];

        //get the closest connector that is able to connect.
        while( Candidates.Count > 0 )
        {
            if( inputsHandler.swapPort() )
            {
                Debug.Log( "Swapping Port" );
                
                PartEnd = Candidates[0];
                
                for( int i = 0; i < Candidates.Count; i++ )
                {
                    if( ( StationEnd.transform.position - Candidates[i].transform.position ).sqrMagnitude < ( StationEnd.transform.position - PartEnd.transform.position ).sqrMagnitude )
                    {
                        PartEnd = Candidates[i];
                    }
                }

                if( TryPlaceTogether( StationEnd, PartEnd, Placing ) )
                {
                    CurrentPort = PartEnd;
                    Candidates.Remove( PartEnd );
                    Debug.Log( "Fits" );
                }
                else
                {
                    Candidates.Remove( PartEnd );
                }

                PartEnd = null;
            }
            
            if ( inputsHandler.finalizeConnection() )
            {
                if ( FinalizeConnection( StationEnd.Connector, PartEnd.Connector, Placing ) != null)
                {
                    yield break;
                }
            }

            yield return null;
        }

        if( Candidates.Count == 0 )
        {
            Debug.Log( "Couldn't find suitable candidate" );
        }
    }

    //Call finalizePlacement?
    public GameObject FinalizeConnection( Connector StationEnd, Connector PartEnd, GameObject Placing )
    {
        if ( Placing == null )
        {
            return null;
        }
        
        StationEnd.Connect( PartEnd );
        
        GameObject FinalPlacement = Instantiate( Placing.gameObject, Placing.Transform.Position, Placing.Transform.Rotation, null );
        
        if ( BuildingPlaced != null )
        {
            BuildingPlaced( FinalPlacement );
        }
        
        Destroy( Placing );
    }

    public bool TryPlaceTogether( ConnectorEnd StationEnd, ConnectorEnd PartEnd, GameObject Placing )
    {
        //check only the part collider.
        Collider checkPlacement = PartEnd.transform.root.GetComponentInChildren<Structure>().PlacementCollider;

        Placing.transform.forward = -StationEnd.transform.forward;
        Placing.transform.eulerAngles += ( Placing.transform.eulerAngles - PartEnd.transform.eulerAngles );
  
        Placing.transform.position += ( StationEnd.transform.position - PartEnd.transform.position );

        List<Collider> inCol = Physics.OverlapBox( checkPlacement.bounds.center, checkPlacement.bounds.extents, Quaternion.identity, ~1 << 1, QueryTriggerInteraction.Ignore ).ToList();
        ExtDebug.DrawBoxCastBox( checkPlacement.bounds.center, checkPlacement.bounds.extents, Quaternion.identity, Vector3.zero, 0f, Color.red );

        for( int i = 0; i < inCol.Count; )
        {
            if( inCol[i].transform.root == Placing.transform )
            {
                inCol.RemoveAt( i );
                continue;
            }

            i++;
        }

        if( inCol.Count > 0 )
        {
            return false;
        }

        return true;
    }
}
