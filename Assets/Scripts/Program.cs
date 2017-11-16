using System;
using System.Collections.Generic;
using System.Linq;
   
class Program
{
    public static void StandardStart()
    {
        Simulation sim = new Simulation();
        sim.Initialize();

        EntityDatabase entityDB = new EntityDatabase();
        ResourceDatabase resDB = new ResourceDatabase();
        StructureDatabase structDB = new StructureDatabase();
    }

    public static void EnterTestValues()
    {
        ResourceDatabase.Instance.Populate();
        StructureDatabase.Instance.Populate();

        Station t = new Station();
        
        //readd buildings
    }
}

