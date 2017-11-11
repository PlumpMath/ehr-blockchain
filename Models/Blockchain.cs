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
        private List<IBlock> _chain; 
        private List<ITransaction> _currentTransactions; 
        private HashSet<string> _nodes; 

        public IBlock LastBlock => _chain[_chain.Count - 1];
        public List<IBlock> Chain
        {
            get => _chain;
            set => _chain = value;
        }
        public List<ITransaction> CurrentTransactions => _currentTransactions;

        public Blockchain()
        {
            _chain = new List<IBlock>();
            _currentTransactions = new List<ITransaction>();
            _nodes = new HashSet<string>();

            NewBlock(1, "100");
        }
        
        public IBlock NewBlock(int proof, string previousHash = null)
        {
            var block = new Block()
            {
                Index = _chain.Count + 1,
                TimeStamp = DateTime.Now,
                Transactions = new List<ITransaction>(_currentTransactions),
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

        public bool ValidChain(List<IBlock> chain)
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
            // var neighbors = _nodes; 
            // var newChain = new List<IBlock>();
            // int maxLength = _chain.Count;

            // foreach (var node in neighbors)
            // {
            //     var url = $"{node}/chain";
            //     HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            //     request.ContentType="application/json; charset=utf-8";
            //     var response = (HttpWebResponse) request.GetResponse();    
            //     string text; 
            //     using (var sr = new StreamReader(response.GetResponseStream()))
            //     {
            //         text = sr.ReadToEnd();
            //     }

            //     var chainobj = JsonConvert.DeserializeObject<BlockchainDto>(text);
            // }

            string node = "http://127.0.0.1:5001";
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