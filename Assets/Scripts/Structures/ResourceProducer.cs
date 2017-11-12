using System;
using System.Collections.Generic;
using System.Linq;

public class ResourceProducer : Structure
{
    new ResourceProducerTemplate Template;
    
    public override void Tick()
    {
        Produce();
    }
    
    public ResourceProducer(ResourceProducerTemplate structureTemplate) : base(structureTemplate)
    {
        Template = structureTemplate;
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
        
        float productionMultiplier = Resource.SmallestNormalizedAvailable(Template.Inputs, resources);
        
        List<Resource> produced = new List<Resource>();
        
        for (int i = 0; i < Template.Inputs.Count; i++)
        {
            float AmountLeft = Template.Inputs[i].Amount;
            
            List<Resource> StoredNeededResource = resources.Where(x => x.Base == Template.Inputs[i].Base).ToList();
            
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
        
        for (int i = 0; i < Template.Outputs.Count; i++)
        {
            produced.Add(Template.Outputs[i].Copy(productionMultiplier));
        }

        MyDebug.DebugWrite("Produced: " + produced.Count);

        Owner.TryDeposit(produced);
    }
}