using System;
using System.Collections.Generic;

#if old
public class OreProcessing : ResourceProducer
{
    public override void Initialize()
    {
        base.Initialize();
        
        base.Outputs = new List<Resource>(
            new Resource[] 
                { 
                    new Resource("Common Metals", 20f)
                }
            );
    }
}
#endif