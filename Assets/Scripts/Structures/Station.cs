using System;
using System.Collections.Generic;
using System.Linq;

public class Station : Structure
{
    public List<Structure> Parts;
    
    public List<T> PartsOfType<T>() where T : Structure
    {
        List<Structure> parts = Parts.Where(x => x.GetType() == typeof(T)).ToList();
        List<T> returnValues = new List<T>();
        
        for (int i = 0 ; i < parts.Count;i++)
        {
            T part = parts[i] as T;
            if (part != null)
            {
                returnValues.Add(part);
            }
        }
        
        return returnValues;
    }
    
    public override void Initialize()
    {
        Owner = this;
        Parts = new List<Structure>();
    }
    
    public void TryDeposit(List<Resource> resources)
    {
        foreach (Resource r in resources)
        {
            TryDeposit(r);
        }
    }
    
    public void TryDeposit(Resource r)
    {
        List<Storage> Storages = PartsOfType<Storage>();
        
        for (int i = 0; i < Storages.Count;i++)
        {
            Storages[i].Deposit(r);
        }
    }
    
    public void Register(Structure structure)
    {
        if (Parts.FirstOrDefault(res => res == structure) != null)
        {
            return;
        }
        
        if (!structure.Initialized)
        {
            structure.Initialize();
        }
        
        structure.Owner = this;
        
        Parts.Add(structure);
        
        structure.OnDestroyed += () => OnPartDestroyedHandler(structure);
    }
    
    public void OnPartDestroyedHandler(Structure part)
    {
         Parts.Remove(part);
    }
}