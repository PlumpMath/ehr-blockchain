using System;

namespace emr_blockchain.Models
{
    public class Transaction : ITransaction
    {
        public string Sender {get; internal set;}
        public string Recipient {get; internal set;}
        public int Amount {get; internal set;}
    }
}