using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ResourceBase : ScriptableObject //ScriptableObject=
{
    //Sprite?
    //Id?
    
    public string Name;
    public string DisplayName;
    
    private ResourceBase(string Name) : this(Name, Name)
    {
        
    }
    
    private ResourceBase(string Name, string DisplayName)
    {
        this.Name = Name;
        this.DisplayName = DisplayName;
    }
    
    public static ResourceBase Get(string Name)
    {
        if (ResourceDatabase.Instance != null)
        {
            ResourceBase match = ResourceDatabase.Instance.Resources.FirstOrDefault(res => res.Name == Name);
            if (match != null)
            {
                return match;
            }
        }
        return null;
    }
    
    public static ResourceBase CreateOrGet(string Name)
    {
        return CreateOrGet(Name, Name);
    }
    
    public static ResourceBase CreateOrGet(string Name, string DisplayName)
    {
        ResourceBase existing = Get(Name);
        if (existing == null)
        {
            existing = new ResourceBase(Name, DisplayName);
        }
        return existing;
    }
}

[System.Serializable]
public class Resource
{
    public readonly ResourceBase Base;
    
    public string Name {get {return Base.Name;}}
    public string DisplayName {get {return Base.DisplayName;}}
    
    public float Amount;
 
    //full copy
    public Resource Copy()
    {
        return new Resource(Base, Amount);   
    }
    
    public Resource Copy(float amount)
    {
        return new Resource(Base, amount);   
    }
 
 
    public Resource(ResourceBase Base) : this(Base, 0)
    {
        
    }
    
    
    public Resource(string Name) : this(Name, 0)
    {
        
    }
    
    
    public Resource(string Name, float Amount) : this(ResourceBase.CreateOrGet(Name), Amount)
    {

    }
    
    
    public Resource(ResourceBase Base, float Amount)
    {
        this.Base = Base;
        this.Amount = Amount;
    }
    

    public bool Equals(Object obj)
    {
        Resource res = obj as Resource; 
        if (res == null)
            return false;
        else
            return Base == res.Base;
    }
    

    public static List<Resource> CombineInstancesOfResources(List<Resource> resources)
    {
        List<Resource> output = new List<Resource>();
        
        for (int i = 0; i < resources.Count; i++)
        {
            if (output.Contains(resources[i]))
            {
                continue;
            }
            
            output.Add(resources[i].Copy(0));
            resources.ForEach(x => 
            {
                if (x.Equals(resources[i]))
                {
                    resources[i].Amount += x.Amount;
                }
            });
        }
        
        return output;
    }

    public static Dictionary<Resource, float> NormalizedAvailableAmounts(List<Resource> needed, List<Resource> available)
    {
        Dictionary<Resource, float> availabilities = new Dictionary<Resource, float>();
        
        List<Resource> Needed = CombineInstancesOfResources(needed);
        List<Resource> Available = CombineInstancesOfResources(available);
        
        for (int i = 0; i < Needed.Count; i++)
        {
            Resource availRes = Available.FirstOrDefault(x => x.Equals(Needed[i]));
            
            float comparison = 0f;
            
            if (availRes != null)
            {
                comparison = ( availRes.amount / Needed[i].Amount);
            }
            
            availabilities.Add(new KeyValuePair(Needed[i].Copy(0), comparison));
        }
        
        return availabilities;
    }
    
    public static float SmallestNormalizedAvailable(List<Resource> needed, List<Resource> available)
    {
        float smallest = 1f;
        
        Dictionary<Resource, float> avail = NormalizedAvailableAmounts(needed, available);
        
        foreach (KeyValuePair k in avail)
        {
            if (k.Value < smallest)
            {
                smallest = k.Value;
            }
        }
        
        return smallest;
    }   
    
    
    public static Resource ListHas(List<Resource> lookIn, string lookFor)
    {
        Resource match = lookIn.FirstOrDefault(res => res.Base.Name == lookFor);
        return match;
    }
    
    public static Resource ListHas(List<Resource> lookIn, Resource lookFor)
    {
        return ListHas(lookIn, lookFor.Name);
    }
}