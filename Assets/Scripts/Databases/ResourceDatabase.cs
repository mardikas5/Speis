using System;
using System.Collections.Generic;

[Serializable]
public class ResourceDatabase : Singleton<ResourceDatabase>
{
    public List<PersistentItem> Resources;
    
    public ResourceDatabase()
    {
        Resources = new List<PersistentItem>();
    }
}