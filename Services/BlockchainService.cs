using System.Collections.Generic;
using System.IO;
using System.Net;
using emr_blockchain.Models;
using emr_blockchain.Models.Dto;
using Newtonsoft.Json;

namespace emr_blockchain.Services
{
    public class BlockchainService : IBlockchainService
    {
        List<IBlock> IBlockchainService.GetChainAtUri(string chainUri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(chainUri);
            request.ContentType="application/json; charset=utf-8";
            var response = (HttpWebResponse) request.GetResponse();    
            string text; 
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            //Handle deserialization and conversion of DTO to model
            var chainDto = JsonConvert.DeserializeObject<BlockchainForUpdateDto>(text);
            var chain = new List<IBlock>();
            foreach (var block in chainDto.Chain)
            {
                var transactions = new List<ITransaction>();
                foreach (var transaction in block.Transactions)
                    transactions.Add(transaction);
                
                chain.Add(new Block()
                {
                    Index = block.Index,
                    TimeStamp = block.TimeStamp,
                    Transactions = transactions,
                    Proof = block.Proof,
                    PreviousHash = block.PreviousHash,
                });
            }

            return chain;
        }

        int IBlockchainService.GetChainLengthAtUri(string chainUri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(chainUri);
            request.ContentType="application/json; charset=utf-8";
            var response = (HttpWebResponse) request.GetResponse();    
            string text; 
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            var chainDto = JsonConvert.DeserializeObject<BlockchainForUpdateDto>(text);
            return chainDto.Length;
        }
    }
}