using System;
using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public class Block : IBlock
    {
        public int Index { get; set; }

        public DateTime TimeStamp { get; set;}

        public List<Dictionary<string, string>> Transactions { get; set;}

        public int Proof { get; set;}

        public int? PreviousHash { get; set;}
    }
}