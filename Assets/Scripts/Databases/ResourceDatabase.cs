using System;
using System.Collections.Generic;

public class ResourceDatabase : Singleton<ResourceDatabase>
{
    public List<ResourceBase> Resources;
    
    public ResourceDatabase()
    {
        Resources = new List<ResourceBase>();
    }
    
    public void Populate()
    {
        Resources.Add(ResourceBase.CreateOrGet("Wood"));
        Resources.Add(ResourceBase.CreateOrGet("Food"));
        Resources.Add(ResourceBase.CreateOrGet("Rare Metals"));
        Resources.Add(ResourceBase.CreateOrGet("Common Metals"));
        Resources.Add(ResourceBase.CreateOrGet("Water"));
    }
}