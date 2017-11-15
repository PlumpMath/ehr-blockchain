using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using emr_blockchain.Models.Dto;

namespace emr_blockchain.Models
{
    public class Blockchain : IBlockchain
    {
        #region Private fields

        private List<Block> _chain; 
        private List<Transaction> _currentTransactions; 
        private HashSet<string> _nodes; 

        #endregion

        #region Properties    
        public HashSet<string> Nodes => _nodes;
        public IBlock LastBlock => _chain[_chain.Count - 1];

        public List<Block> Chain
        {
            get => _chain;
            set => _chain = value;
        }

        public List<Transaction> CurrentTransactions => _currentTransactions;

        #endregion
        public Blockchain()
        {
            _chain = new List<Block>();
            _currentTransactions = new List<Transaction>();
            _nodes = new HashSet<string>();

            NewBlock(1, "100");
        }

        public void RegisterNode(string address)
        {
            _nodes.Add(address);
        }
        
        public IBlock NewBlock(int proof, string previousHash = null)
        {
            var block = new Block()
            {
                Index = _chain.Count + 1,
                TimeStamp = DateTime.Now.ToString(),
                Transactions = new List<Transaction>(_currentTransactions),
                Proof = proof, 
                PreviousHash = previousHash ?? Hash(LastBlock),
            };

            _chain.Add(block);
            _currentTransactions.Clear();
            return block;
        }

        public int NewTransaction(string sender, string recipient, int amount)
        {
            _currentTransactions.Add(new Transaction()
            {
                Sender = sender,
                Recipient = recipient,
                Amount = amount
            });

            return LastBlock.Index + 1;
        }

        public bool ValidChain(List<Block> chain)
        {
            var lastBlock = chain[0];
            int currentIndex = 1; 
            while (currentIndex < chain.Count)
            {
                var block = chain[currentIndex];
                if (block.PreviousHash != Hash(lastBlock))
                    return false;
                lastBlock  = block; 
                currentIndex++;
            }

            return true;
        }

        public bool ResolveConflicts()
        {
            var neighbors = _nodes; 
            var newChain = new List<Block>();
            int maxLength = _chain.Count;

            foreach (var node in neighbors)
            {
                var url = $"{node}/chain";
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.ContentType="application/json; charset=utf-8";
                var response = (HttpWebResponse) request.GetResponse();    
                string text; 
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                }

                var chainobj = JsonConvert.DeserializeObject<BlockchainDto>(text);

                var chain = chainobj.Chain;
                var length = chainobj.Length;

                if (length > maxLength && ValidChain(chain))
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
    }
}