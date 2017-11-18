using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ResourceProducer : StructureBehaviour
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
    }
    
    public void Produce()
    {
        if (Owner == null)
        {
            return;
        }
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
            
            List<Resource> StoredNeededResource = resources.Where(x => x.Base == Inputs[i].Base).ToList();
            
            for (int k = 0; k < StoredNeededResource.Count; k++)
            {
                if (StoredNeededResource[k].Amount > AmountLeft)
                {
                    StoredNeededResource[k].Amount -= AmountLeft;
                    AmountLeft = 0;
                    break;
                }
                AmountLeft -= StoredNeededResource[k].Amount;
                StoredNeededResource[k].Amount = 0;
            }
            
            if (AmountLeft > 0)
            {
                MyDebug.DebugWrite("Amounts not matching, ResourceProducer debug");
            }
        }
        
        for (int i = 0; i < Outputs.Count; i++)
        {
            produced.Add(Outputs[i].Copy(productionMultiplier));
        }

        MyDebug.DebugWrite("Produced: " + produced.Count);

        Owner.TryDeposit(produced);
    }
}