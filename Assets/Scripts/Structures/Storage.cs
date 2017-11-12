using System;
using System.Collections.Generic;

public class Storage : Structure, ITransactionable
{
    public List<Resource> Stored {get; set;}
    
    public float Volume = 300;
    
    public Storage(StorageTemplate storageTemplate) : base(storageTemplate)
    {
        Volume = storageTemplate.Volume;
    }

    public override void Initialize()
    {
        base.Initialize();
        Stored = new List<Resource>();
    }
    
    public Resource Get(string Name)
    {
        return Get(Name, false);
    }
    
    public Resource Get(string Name, bool Create = false)
    {
        if (Stored != null)
        {
            Resource match = Resource.ListHas(Stored, Name);
            if (match != null)
            {
                return match;
            }
        }
        if (Create)
        {
            Resource created = new Resource(Name);
            Stored.Add(created);
            return created;
        }
        return null;
    }
    
    //change to return resource incase left over.
    public void Deposit(Resource res)
    {
        Resource inStore = Get(res.Name, res.Amount > 0);
        if (res.Amount < 0)
        {
            return;
        }
        if (inStore != null)
        {
            inStore.Amount += res.Amount;
        }
    }
    
    public void Deposit(string Name, float Amount)
    {
        Deposit(new Resource(Name, Amount));
    }
    
    public Resource Withdraw(Resource res)
    {
        return Withdraw(res.Base.Name, res.Amount);
    }
    
    public Resource Withdraw(ResourceBase Base, float Amount)
    {
        return Withdraw(Base.Name, Amount);
    }
    
    public Resource Withdraw(string Name, float Amount)
    {
        Resource inStore = Get(Name);
        
        Resource outp;
        
        if (inStore == null)
        {
            return null;
        }
        
        if (inStore.Amount <= Amount)
        {
            outp = new Resource(inStore.Base, Amount);
            Stored.Remove(inStore);
        }
        else
        {
            outp = new Resource(inStore.Base, inStore.Amount);
        }
        
        inStore.Amount -= outp.Amount;
        
        return outp;
    }
}