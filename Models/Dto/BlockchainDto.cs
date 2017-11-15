using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace emr_blockchain.Models.Dto
{
    public class BlockchainDto
    {
        private IBlockchain _chain; 
        
        public BlockchainDto()
        {
            _chain = new Blockchain();
            _chain.Chain = new List<Block>();
        }

        public BlockchainDto(IBlockchain chain) 
        {
            _chain = chain;
        }

        [Required]
        public List<Block> Chain 
        {
            get => _chain.Chain;
        } 
        
        [Required]
        public int Length 
        {
            get => _chain.Chain.Count;
        }
    }
}