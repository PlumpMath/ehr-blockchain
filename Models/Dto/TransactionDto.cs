using System.ComponentModel.DataAnnotations;

namespace emr_blockchain.Models.Dto
{
    public class TransactionDto
    {
        [Required]
        public string Sender {get; set;}

        [Required]
        public string Recipient {get; set;}

        [Required]
        public int Amount {get; set;}
    }
}