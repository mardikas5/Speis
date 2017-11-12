using System;
using System.Collections.Generic;
using System.Linq;

public class ResourceProducer : Structure
{
    public List<Resource> Inputs;
    public List<Resource> Outputs;
    
    public override void Tick()
    {
        Produce();
    }
    
    public override void Initialize()
    {
        base.Initialize();
        Inputs = new List<Resource>();
        Outputs = new List<Resource>();
    }
    
    public void Produce()
    {
        List<Storage> Storages = Owner.PartsOfType<Storage>();

        List<Resource> resources = new List<Resource>();
        
        for (int i = 0; i < Storages.Count; i++)
        {
            resources.AddRange(Storages[i].Stored);
        }
        
        float productionMultiplier = Resource.SmallestNormalizedAvailable(Inputs, resources);
        
        List<Resource> produced = new List<Resource>();
        
        for (int i = 0; i < Inputs.Count; i++)
        {
            float AmountLeft = Inputs[i].Amount;
            
            List<Resource> Instances = resources.Where(x => x.Base == Inputs[i].Base).ToList();
            
            for (int k = 0; k < Instances.Count; k++)
            {
                if (Instances[k].Amount > AmountLeft)
                {
                    Instances[k].Amount -= AmountLeft;
                    AmountLeft = 0;
                    break;
                }
                AmountLeft -= Instances[k].Amount;
                Instances[k].Amount = 0;
            }
            
            if (AmountLeft > 0)
            {
                Console.WriteLine("Amounts not matching, ResourceProducer debug");
            }
        }
        
        for (int i = 0; i < Outputs.Count; i++)
        {
            produced.Add(Outputs[i].Copy(productionMultiplier));
        }
        Console.WriteLine("Produced: " + produced.Count);
        Owner.TryDeposit(produced);
    }
}