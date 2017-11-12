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
        ResourceProducer Producer = new ResourceProducer();
        Producer.Name = "Mining Bay";
        Producer.Inputs = null;
        
        Producer.BuildingCost = new List<Resource>(new Resource[] { new Resource("Wood", 25) 
        });
        
        Producer.Outputs = new List<Resource>(new Resource[] { new Resource("Common Metals", 5)
        });

        
        Structures.Add(Producer);
        
        Producer = new ResourceProducer();
        Producer.Name = "GreenHouse";
        
        Producer = new ResourceProducer();
        Producer.Name = "Atmosphere Recycling";
        
        
    }
}