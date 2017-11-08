using System.Collections.Generic;

namespace emr_blockchain.Models.Dto
{
    public class BlockDto
    {
        private IBlock _block; 

        public BlockDto(IBlock block, string message)
        {
            _block = block;
            Message = message;
        }

        public string Message {get;}

        public int Index => _block.Index;

        public int Proof => _block.Proof;

        public string PreviousHash => _block.PreviousHash;

        public ICollection<Dictionary<string, string>> Transactions {get; set;}

    }
}