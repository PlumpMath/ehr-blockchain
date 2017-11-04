using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public interface IBlockchain
    {
        ICollection<IBlock> Chain {get;}
    }
}