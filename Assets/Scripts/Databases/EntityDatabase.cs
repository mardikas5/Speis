using System;
using System.Collections.Generic;

public class EntityDatabase : Singleton<EntityDatabase>
{
    public List<Entity> entities;
    
    public EntityDatabase()
    {
        entities = new List<Entity>();
    }
}