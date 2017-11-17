using System.Collections.Generic;

namespace emr_blockchain.Models
{
    public interface IBlockchain
    {
        #region Properties
        HashSet<string> Nodes {get;}

        List<ITransaction> CurrentTransactions {get;}

        List<IBlock> Chain {get;}

        IBlock LastBlock {get;}

        #endregion

        #region Methods
        void RegisterNode(string address);

        IBlock CreateBlock(int proof, string previousHash);

        int AddNewTransaction(string sender, string recipient, int amount);

        bool ValidateChain(List<IBlock> chain);

        bool ResolveConflicts();

        int ProofOfWork(int lastProof);

        #endregion
    }
}