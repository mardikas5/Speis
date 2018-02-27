using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameLogic : Singleton<GameLogic>
{
    public bool PauseInteractable = true;

    public int TicksPerSecond = 5;

    private float TickRate
    {
        get
        {
            return 1f / (float)TicksPerSecond;
        }
    }


    // Use this for initialization
    public override void Start()
    {
        base.Start();

        DontDestroyOnLoad( this.gameObject );
    }


    public virtual void OnGameStart()
    {
        StandardStart();
        EnterTestValues();
        StartCoroutine( Run() );
    }


    public IEnumerator Run()
    {
        while ( true )
        {
            Simulation.Instance.Tick();

            yield return new WaitForSeconds( TickRate );
        }
    }


    public static void StandardStart()
    {
        Simulation sim = Simulation.Instance;

        sim.Initialize();

        //EntityDatabase entityDB = new EntityDatabase();
        //ResourceDatabase resDB = new ResourceDatabase();
        //StructureDatabase structDB = new StructureDatabase();
    }

    public static void EnterTestValues()
    {
        //ResourceBase[] Found = Resources.FindObjectsOfTypeAll<ResourceBase>();
        PersistentItem[] Found = Resources.LoadAll<PersistentItem>( "ResourceObjects" );
        Debug.Log( "Number of resources loaded: " + Found.Length );
        ResourceDatabase.Instance.Resources.AddRange( Found );
        StructureDatabase.Instance.Populate();

        //Station t = Structure.Create<Station>();
        //Structure Bboi = Structure.Create<Structure>();
        //t.Register( Bboi );
        //Storage stored = Bboi.AddBehaviour<Storage>();
        //stored.Stored.Add( new Resource( ResourceDatabase.Instance.Resources[0], 200 ) );


        //readd buildings
    }
}
