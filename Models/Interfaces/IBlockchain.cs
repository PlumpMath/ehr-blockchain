using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public interface IBlockchain
    {
        List<IBlock> Chain {get; set;}
    }
}