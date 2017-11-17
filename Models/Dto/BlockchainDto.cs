using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emr_blockchain.Models.Dto
{
    public class BlockchainDto
    {
        private IBlockchain _blockchain;
        public BlockchainDto(IBlockchain blockchain)
        {
            _blockchain = blockchain;
        }

        public List<IBlock> Chain => _blockchain.Chain;

        public int Length => Chain.Count;


    }
}