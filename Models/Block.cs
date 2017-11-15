using System;
using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public class Block : IBlock
    {
        public int Index { get; set; }

        public string TimeStamp { get; set;}

        public List<Transaction> Transactions { get; set;}

        public int Proof { get; set;}

        public string PreviousHash { get; set;}
    }
}