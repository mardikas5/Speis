using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlacementObj
{
    public Dictionary<Transform, int> LayermaskValues;

    public GameObject Placement;

    public PlacementObj( GameObject p )
    {
        Placement = p;
        LayermaskValues = new Dictionary<Transform, int>();
        foreach( Collider t in p.gameObject.GetComponentsInChildren<Collider>().ToList() )
        {
            LayermaskValues.Add( t.transform, t.gameObject.layer );
        }
    }

    public void Destroy()
    {
        GameObject.Destroy( Placement );
    }

    public void Restore()
    {
        foreach( KeyValuePair<Transform, int> t in LayermaskValues )
        {
            t.Key.gameObject.layer = t.Value;
        }
    }
}

//namespace Structures
public class Placement : Singleton<Placement>
{
    [Serializable]
    public class InputHandler
    {
        public KeyCode startPlacementKey = KeyCode.R;
        public KeyCode swapPortKey = KeyCode.H;
        public KeyCode finalizeConnectionKey = KeyCode.J;
        public KeyCode endPlacementKey = KeyCode.Escape;

        public Func<bool> startPlacement;
        public Func<bool> swapPort;
        public Func<bool> finalizeConnection;
        public Func<bool> endPlacement;

        public void Init()
        {
            startPlacement = () => isKeyDown( startPlacementKey );
            swapPort = () => isKeyDown( swapPortKey );
            finalizeConnection = () => isKeyDown( finalizeConnectionKey );
            endPlacement = () => isKeyDown( endPlacementKey );
        }

        public bool isKeyDown( KeyCode k )
        {
            return Input.GetKeyDown( k );
        }
    }

    [SerializeField]
    public InputHandler inputHandler;

    public GameObject testObject;
    //public GameObject Placing;
    public PlacementObj pObj;

    public bool placementActive;

    public Camera placementCam;

    public Coroutine PlacementCoroutine;

    public event Action<GameObject> BuildingPlaced;

    public void SetPlacing( GameObject placing )
    {
        GameObject Placing = Instantiate( placing );
        pObj = new PlacementObj( Placing );
        Placing.gameObject.layer = 1 << 1;
        Placing.gameObject.GetComponentsInChildren<Collider>().ToList().ForEach( x => x.gameObject.layer = 1 << 1 );
    }

    public override void Start()
    {
        base.Start();
        inputHandler.Init();
    }

    void Update()
    {
        UpdatePlacement();
    }


    public void UpdatePlacement()
    {
        placementActive = PlacementCoroutine != null;

        if( inputHandler.startPlacement() )
        {
            StartPlacementHandler();
        }
        if( inputHandler.endPlacement() )
        {
            BreakPlacement();
        }
    }

    private void StartPlacementHandler()
    {
        if( pObj == null )
        {
            return;
        }
        if( pObj.Placement == null )
        {
            SetPlacing( testObject );
        }

        if( PlacementCoroutine == null && pObj != null )
        {
            Debug.Log( "Placing" );
            PlacementCoroutine = StartCoroutine( TryConnectRoutine( 5f, pObj, () => { PlacementCoroutine = null; pObj = null; } ) );
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


    public IEnumerator TryConnectRoutine( float distance, PlacementObj placing, Action onCompleted = null )
    {
        //ConnectorEnd ConnectToStation = null;
        ConnectorEnd PartEnd = null;
        ConnectorEnd StationEnd = GetConnectorAtScreenPoint( Input.mousePosition, distance );
        int partIndex = -1;

        if( StationEnd == null || placing.Placement == null )
        {
            yield break;
        }

        List<ConnectorEnd> Candidates = placing.Placement.transform.root.GetComponentsInChildren<ConnectorEnd>().ToList();

        PartEnd = Candidates[0];

        for( int i = 0; i < Candidates.Count; i++ )
        {
            if( !TryPlaceTogether( StationEnd, Candidates[i], placing.Placement ) )
            {
                Candidates.RemoveAt( i );
                i--;
            }
        }

        //get the closest connector that is able to connect.
        while( Candidates.Count > 0 && pObj != null && pObj.Placement != null )
        {
            if( inputHandler.swapPort() )
            {
                partIndex++;

                if( partIndex >= Candidates.Count )
                {
                    partIndex = 0;
                }

                PartEnd = Candidates[partIndex];

                TryPlaceTogether( StationEnd, PartEnd, placing.Placement );
            }

            if( inputHandler.finalizeConnection() )
            {
                if( FinalizeConnection( StationEnd.connector, PartEnd.connector, placing ) != null )
                {
                    break;
                }
            }

            yield return null;
        }

        if( Candidates.Count == 0 )
        {
            Debug.Log( "Couldn't find suitable candidate" );
            BreakPlacement();
        }

        if( onCompleted != null )
        {
            onCompleted();
        }
    }

    //Call finalizePlacement?
    public GameObject FinalizeConnection( Connector StationEnd, Connector PartEnd, PlacementObj placing )
    {
        if( placing == null )
        {
            return null;
        }

        StationEnd.Connect( PartEnd );

        if( BuildingPlaced != null )
        {
            BuildingPlaced( placing.Placement );
        }

        placing.Restore();

        return placing.Placement;
    }

    public void BreakPlacement()
    {
        if( pObj == null )
        {
            return;
        }
        pObj.Destroy();
        pObj = null;
    }

    public void RestorePlacement()
    {

    }

    /// <summary>
    /// Tries to fit the connectors together in the game world.
    /// </summary>
    /// <param name="StationEnd"></param>
    /// <param name="PartEnd"></param>
    /// <param name="Placing"></param>
    /// <returns>True if parts fit together.</returns>
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
