using System.Collections.Generic;
using System.Linq;

namespace emr_blockchain.Models.Dto
{
    public class BlockchainDto
    {
        private IBlockchain _chain; 
        public BlockchainDto(IBlockchain chain) 
        {
            _chain = chain;
        }

        public ICollection<IBlock> chain => _chain.Chain;
        public int length => _chain.Chain.ToList().Count;
    }
}