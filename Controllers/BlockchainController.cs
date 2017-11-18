using Microsoft.AspNetCore.Mvc;
using emr_blockchain.Models;
using emr_blockchain.Models.Dto;
using System;
using System.Collections.Generic;

namespace emr_blockchain.Controllers
{
    public class BlockchainController : Controller
    {
        private static List<ITransaction> _transaction = new List<ITransaction>();
        private static List<IBlock> _chain = new List<IBlock>();
        private static Blockchain _blockchain = new Blockchain(_chain, _transaction);

        [HttpGet("/mine")]
        public IActionResult MineBlock()
        {
            var lastBlock = _blockchain.LastBlock;
            var lastProof = lastBlock.Proof;
            var proof = _blockchain.ProofOfWork(lastProof);

            _blockchain.AddNewTransaction("0", "unique" ,1);
            _blockchain.CreateBlock(proof);
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
            var chainDto = new BlockchainDto(_blockchain);
            return Ok(chainDto);
        }

        [HttpPost("/newtransaction")]
        public IActionResult AddNewTransaction([FromBody] TransactionDto transaction)
        {
            if (transaction == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            _blockchain.AddNewTransaction(transaction.Sender, transaction.Recipient, transaction.Amount);
            return Created("/newtransaction", 
            new 
            { 
                Message = "Transaction created",
                Sender = transaction.Sender,
                Recipient = transaction.Recipient, 
                Amount = transaction.Amount
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

            return Ok(new {Message = "Our chain is authoritative", NewChain= _blockchain.Chain});
        }

        [HttpGet("/debug")]
        public IActionResult Debug()
        {
            _blockchain.ResolveConflicts();
            return Ok();
        }
    }
}