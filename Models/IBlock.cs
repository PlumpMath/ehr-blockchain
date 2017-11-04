using System;
using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public interface IBlock
    {
        int Index { get; set;}

        DateTime TimeStamp { get; set;}

        List<Dictionary<string, string>> Transactions { get; set;}

        int Proof { get; set;}

        int? PreviousHash { get; set;}
    }    
}