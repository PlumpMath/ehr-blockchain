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

        [HttpPost("/nodes/register")]
        public IActionResult RegisterNodes([FromBody] RegisteredNodesDto nodes)
        {
            if (nodes == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            foreach (var node in nodes.Nodes) 
                _blockchain.RegisterNode(node);

            return Created("/nodes/register", new {Message = "New nodes have been added", Nodes = _blockchain.Nodes});
        }

        [HttpGet("/nodes/resolve")]
        public IActionResult Consensus()
        {
            var chainReplaced = _blockchain.ResolveConflicts();

            if (chainReplaced)
                return Ok(new {Message = "Our chain was replaced", NewChain = _blockchain.Chain} );

            return Ok(new {Message = "Our chain is authoritative", NewChain=_blockchain.Chain});
        }

        [HttpPost("/debug")]
        public IActionResult Debug([FromBody] DebugBlockchainDto chain)
        {
            if (chain == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(chain);
        }

        [HttpGet("/debugthis")]
        public IActionResult DebugThis()
        {
            _blockchain.ResolveConflicts();
            return Ok();
        }
    }
}