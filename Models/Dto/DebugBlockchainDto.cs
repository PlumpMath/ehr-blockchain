using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emr_blockchain.Models.Dto
{
    public class DebugBlockchainDto
    {
        [Required]
        public List<Block> Chain 
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