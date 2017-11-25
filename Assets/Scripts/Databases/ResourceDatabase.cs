using System;
using System.Collections.Generic;

[Serializable]
public class ResourceDatabase : Singleton<ResourceDatabase>
{
    public List<ResourceBase> Resources;
    
    public ResourceDatabase()
    {
        Resources = new List<ResourceBase>();
    }
}