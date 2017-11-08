using System;

namespace emr_blockchain.Models
{
    public interface ITransaction
    {
        string Sender {get;}
        string Recipient {get;}
        int Amount {get;}
    }
}