using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emr_blockchain.Models.Dto
{
    public class BlockchainForUpdateDto
    {
        [Required]
        public List<BlockDto> Chain 
        {
            get; set;
        }
        
        [Required]
        public int Length 
        {
           get; set;
        }
    }
}