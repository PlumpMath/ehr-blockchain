using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public interface IBlockchain
    {
        List<Block> Chain {get; set;}
    }
}