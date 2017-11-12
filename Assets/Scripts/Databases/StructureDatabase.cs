using System;
using System.Collections.Generic;
using System.Linq;

public class StructureDatabase : Singleton<StructureDatabase>
{
    public List<StructureTemplate> Structures = new List<StructureTemplate>();
    
    public StructureDatabase()
    {
        
    }
    
    public void Populate()
    {
        ResourceProducerTemplate Producer = new ResourceProducerTemplate();
        Producer.Name = "Mining Bay";
        Producer.Inputs = null;
        
        Producer.BuildingCost = new List<Resource>(new Resource[] { new Resource("Wood", 25) 
        });
        
        Producer.Outputs = new List<Resource>(new Resource[] { new Resource("Common Metals", 5)
        });

        
        Structures.Add(Producer);
        
        Producer = new ResourceProducerTemplate();
        Producer.Name = "GreenHouse";
        
        Producer = new ResourceProducerTemplate();
        Producer.Name = "Atmosphere Recycling";
        
        
    }
}