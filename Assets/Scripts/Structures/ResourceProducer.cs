using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ResourceProducer : StructureBehaviour
{
    public List<Storable> Inputs;
    public List<Storable> Outputs;
    public float LastProdMultiplier = 0f;

    public override void Tick()
    {
        base.Tick();
        if( Enabled )
        {
            Produce();
        }
    }

    public void Produce()
    {
        if( Structure.Owner == null )
        {
            return;
        }
        List<Storage> Storages = Structure.Owner.PartsOfType<Storage>();

        List<Storable> resources = new List<Storable>();

        for( int i = 0; i < Storages.Count; i++ )
        {
            resources.AddRange( Storages[i].Stored );
        }

        float productionMultiplier = Storable.SmallestNormalizedAvailable( Inputs, resources );

        List<Storable> produced = new List<Storable>();

        for( int i = 0; i < Inputs.Count; i++ )
        {
            if( Inputs[i] == null || Inputs[i].Base == null )
            {
                continue;
            }
            float AmountLeft = Inputs[i].Amount;

            List<Storable> StoredNeededResource = resources.Where( x => x.Base == Inputs[i].Base ).ToList();

            for( int k = 0; k < StoredNeededResource.Count; k++ )
            {
                if( StoredNeededResource[k].Amount > AmountLeft )
                {
                    StoredNeededResource[k].Amount -= AmountLeft;
                    AmountLeft = 0;
                    break;
                }
                AmountLeft -= StoredNeededResource[k].Amount;
                StoredNeededResource[k].Amount = 0;
            }

            if( AmountLeft > 0 )
            {
                Debug.Log( "Amounts not matching, ResourceProducer debug" );
            }
        }

        LastProdMultiplier = productionMultiplier;

        for( int i = 0; i < Outputs.Count; i++ )
        {
            produced.Add( Outputs[i].Copy( Outputs[i].Amount * productionMultiplier ) );
        }

        //MyDebug.DebugWrite( "Produced: " + produced.Count );

        Structure.Owner.TryDeposit( produced );
    }
}