using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public List<IBlock> Chain => _chain.Chain;
        
        [Required]
        public int Length => _chain.Chain.Count;
    }
}