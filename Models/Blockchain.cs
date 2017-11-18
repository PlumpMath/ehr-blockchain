using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using emr_blockchain.Models.Dto;
using emr_blockchain.Services;

namespace emr_blockchain.Models
{
    public class Blockchain : IBlockchain
    {
        #region Private Fields

        private List<IBlock> _chain; 

        private List<ITransaction> _currentTransactions; 

        private HashSet<string> _nodes; 
        
        private IBlockchainService _blockchainService;

        #endregion

        #region Properties    

        public HashSet<string> Nodes => _nodes;

        public IBlock LastBlock => _chain[_chain.Count - 1];

        public List<IBlock> Chain => _chain;

        public List<ITransaction> CurrentTransactions => _currentTransactions;

        #endregion

        #region Constructor

        public Blockchain(List<IBlock> chain, List<ITransaction> currentTransactions, IBlockchainService service)
        {
            _chain = chain;
            _currentTransactions = currentTransactions;
            _nodes = new HashSet<string>();
            _blockchainService = service;

            CreateBlock(1, "100");
        }

        #endregion

        #region Public methods

        public void RegisterNode(string address)
        {
            _nodes.Add(address);
        }
        
        public IBlock CreateBlock(int proof, string previousHash = null)
        {
            var block = new Block()
            {
                Index = _chain.Count + 1,
                TimeStamp = DateTime.Now.ToString(),
                Transactions = new List<ITransaction>(_currentTransactions),
                Proof = proof, 
                PreviousHash = previousHash ?? Hash(LastBlock),
            };

            _chain.Add(block);
            _currentTransactions.Clear();
            return block;
        }

        public int AddNewTransaction(string sender, string recipient, int amount)
        {
            _currentTransactions.Add(new Transaction()
            {
                Sender = sender,
                Recipient = recipient,
                Amount = amount
            });

            return LastBlock.Index + 1;
        }

        public bool ValidateChain(List<IBlock> chain)
        {
            var lastBlock = chain[0];
            int currentIndex = 1; 
            while (currentIndex < chain.Count)
            {
                var block = chain[currentIndex];
                if (block.PreviousHash != Hash(lastBlock))
                {
                    var hash = Hash(lastBlock);
                    return false;
                }
                lastBlock  = block; 
                currentIndex++;
            }

            return true;
        }

        public bool ResolveConflicts()
        {
            var newChain = new List<IBlock>();
            int maxLength = _chain.Count;

            foreach (var node in _nodes)
            {
                var chain = _blockchainService.GetChainAtUri($"{node}/chain");
                var length = _blockchainService.GetChainLengthAtUri($"{node}/chain");
                
                if (length > maxLength && ValidateChain(chain))
                {
                    maxLength = length; 
                    newChain = chain;
                }
            }

            if (newChain.Count > 0)
            {
                _chain = newChain;
                return true;
            }

            return false;
        }

        public int ProofOfWork(int lastProof)
        {
            int proof = 0; 
            while (IsValidProof(lastProof, proof) == false)
                proof++;
            return proof; 
        }

        public static string Hash(IBlock block)
        {
            var blockstring = JsonConvert.SerializeObject(block);
            StringBuilder sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(blockstring));
                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
        #endregion

        #region Private Helper Methods
        private static bool IsValidProof(int prevProof, int proof)
        {
            var guess = Encoding.UTF8.GetBytes(String.Format("{0}{1}", prevProof, proof));
            StringBuilder sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                var result = hash.ComputeHash(guess);
                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }

            var guessHash = sb.ToString();
            return guessHash.Substring(guessHash.Length - 4) == "0000";
        }
        #endregion

    }
}