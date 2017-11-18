using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emr_blockchain.Models.Dto
{

    public class BlockDto
    {
        public int Index { get; set; }

        public string TimeStamp { get; set;}

        public List<Transaction> Transactions { get; set;}

        public int Proof { get; set;}

        public string PreviousHash { get; set;}
    }
}