using System;
using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public interface IBlock
    {
        int Index { get; set;}

        DateTime TimeStamp { get; set;}

        List<ITransaction> Transactions { get; set;}

        int Proof { get; set;}

        string PreviousHash { get; set;}
    }    
}