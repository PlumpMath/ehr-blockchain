using Microsoft.AspNetCore.Mvc;
using emr_blockchain.Models;
using emr_blockchain.Models.Dto;
using System;

namespace emr_blockchain.Controllers
{
    public class BlockchainController : Controller
    {
        private static Blockchain _blockchain = new Blockchain();

        [HttpGet("/mine")]
        public IActionResult MineBlock()
        {
            var lastBlock = _blockchain.LastBlock;
            var lastProof = lastBlock.Proof;
            var proof = _blockchain.ProofOfWork(lastProof);

            _blockchain.NewTransaction("0", "unique" ,1);
            _blockchain.NewBlock(proof);
            return Ok(_blockchain.LastBlock);
        }

        [HttpGet("/currenttransactions")]
        public IActionResult GetCurrentTransactions()
        {
            return Ok(_blockchain.CurrentTransactions);
        }

        [HttpGet("/chain")]
        public IActionResult GetBlockchain()
        {
            var dto = new BlockchainDto(_blockchain);
            return Ok(dto);
        }

        [HttpPost("/newtransaction")]
        public IActionResult AddNewTransaction([FromBody] TransactionDto transaction)
        {
            if (transaction == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            _blockchain.NewTransaction(transaction.Sender, transaction.Recipient, transaction.Amount);
            return Created("/newtransaction", 
            new 
            { 
                Message = "Transaction created",
                Sender = transaction.Sender,
                Recipient = transaction.Recipient, 
                Amount=transaction.Amount
            });
        }

        [HttpGet("/debug")]
        public IActionResult Debug()
        {
            return Ok();
        }
    }
}