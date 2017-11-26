using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    public ResourceDatabase p = null;
    public int TicksPerSecond = 5;

    private float TickRate
    {
        get
        {
            return 1f / (float)TicksPerSecond;
        }
    }


    // Use this for initialization
    void Start()
    {
        StandardStart();
        EnterTestValues();
        p = ResourceDatabase.Instance;
        StartCoroutine( Run() );
    }

    public IEnumerator Run()
    {
        while( true )
        {
            Simulation.Instance.Tick();

            yield return new WaitForSeconds( TickRate );
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void StandardStart()
    {
        Simulation sim = new Simulation();
        sim.Initialize();
        foreach (Entity t in FindObjectsOfType<Entity>())
        {
            sim.Entities.Add(t);
            t.Initialize();
        }


        EntityDatabase entityDB = new EntityDatabase();
        ResourceDatabase resDB = new ResourceDatabase();
        StructureDatabase structDB = new StructureDatabase();
    }

    public static void EnterTestValues()
    {
        ResourceBase[] Found = Resources.FindObjectsOfTypeAll<ResourceBase>();
        Debug.Log( Found.Length );
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
