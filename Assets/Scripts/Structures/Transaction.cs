using System;
using System.Collections.Generic;
using System.Linq;


public class Transaction
{
    public void Transfer<T>(T From, T To, Resource Transferred) where T : ITransactionable
    {
        Resource inStore = From.Get(Transferred.Name);
        Resource inDeposit = To.Get(Transferred.Name);
        
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
    List<Resource> Stored {get; set;}
    
    void Deposit(Resource res);
    
    Resource Withdraw(Resource res);
    
    Resource Get(string Name);
}