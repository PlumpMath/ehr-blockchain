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
        public IActionResult Mine()
        {
            var lastBlock = _blockchain.LastBlock;
            var lastProof = lastBlock.Proof;
            var proof = _blockchain.ProofOfWork(lastProof);

            _blockchain.NewTransaction("0", "unique" ,1);
            _blockchain.NewBlock(proof);
            return Ok(_blockchain.LastBlock);
        }

        [HttpGet("/chain")]
        public IActionResult FullChain()
        {
            var dto = new BlockchainDto(_blockchain);
            return Ok(dto);
        }
    }
}