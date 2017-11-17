using System;

namespace emr_blockchain.Models
{
    public class Transaction : ITransaction
    {
        public string Sender {get; set;}
        public string Recipient {get; set;}
        public int Amount {get; set;}
    }
}