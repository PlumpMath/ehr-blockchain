using System;
using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public interface IBlock
    {
        int Index { get; set;}

        string TimeStamp { get; set;}

        List<Transaction> Transactions { get; set;}

        int Proof { get; set;}

        string PreviousHash { get; set;}
    }    
}