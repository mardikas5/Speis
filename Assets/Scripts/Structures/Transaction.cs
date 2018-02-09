using System;
using System.Collections.Generic;
using System.Linq;


public class Transaction
{
    public void Transfer<T>(T From, T To, Storable Transferred) where T : ITransactionable
    {
        Storable inStore = From.Get(Transferred.Name);
        Storable inDeposit = To.Get(Transferred.Name);
        
        if (Transferred.Amount <= 0)
        {
            return;
        }
        
        if (inStore == null || inStore.Amount == 0)
        {
            return;
        }
        
        if (inDeposit == null)
        {
            return;
        }
        
        To.Deposit(From.Withdraw(Transferred));
    }
}

public interface ITransactionable
{
    List<Storable> Stored {get; set;}
    
    /// <summary>
    /// Deposits the resource.
    /// </summary>
    /// <param name="res"></param>
    /// <returns>Resource left over.</returns>
    Storable Deposit(Storable res);
    
    Storable Withdraw(Storable res);
    
    Storable Get(string Name);
}