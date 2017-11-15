
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace emr_blockchain.Models.Dto
{
    public class RegisteredNodesDto
    {
        [Required]
        public List<string> Nodes {get; set;}
    }
}