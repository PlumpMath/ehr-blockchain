using System.Collections.Generic;
using emr_blockchain.Models;

namespace emr_blockchain.Services
{
    public interface IBlockchainService
    {
        List<IBlock> GetChainAtUri(string chainUri);

        int GetChainLengthAtUri(string chainUri);
    }
}
