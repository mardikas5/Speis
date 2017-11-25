using System;
using System.Collections.Generic;
using System.Linq;

public class StructureDatabase : Singleton<StructureDatabase>
{
    public List<Structure> Structures = new List<Structure>();
    
    public StructureDatabase()
    {
        
    }
    
    public void Populate()
    {
        //structure mining = new structure();
        //mining.name = "mining bay";
        //resourceproducer producer = new resourceproducer();
        //producer.name = "mining bay drilling";
        //storage storage = new storage(300f);
        //storage.name = "mining bay storage";

        //producer.inputs = null;
       
        //producer.outputs = new list<resource>(new resource[] { new resource("common metals", 5)
        //});

        //mining.addbehaviour<resourceproducer>(producer);
        //mining.addbehaviour<storage>(storage);
        //structures.add(mining);
    }
}