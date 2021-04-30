﻿using Grpc.Core;
using Hashgraph.Implementation;
using System;
using System.Threading;

namespace Proto
{
    public sealed partial class TransactionGetRecordQuery : INetworkQuery
    {
        Query INetworkQuery.CreateEnvelope()
        {
            return new Query { TransactionGetRecord = this };
        }

        Func<Query, Metadata?, DateTime?, CancellationToken, AsyncUnaryCall<Response>> INetworkQuery.InstantiateNetworkRequestMethod(Channel channel)
        {
            return new CryptoService.CryptoServiceClient(channel).getTxRecordByTxIDAsync;
        }

        void INetworkQuery.SetHeader(QueryHeader header)
        {
            Header = header;
        }

        internal TransactionGetRecordQuery(TransactionID transactionRecordId, bool includeDuplicates)
        {
            TransactionID = transactionRecordId;
            IncludeDuplicates = includeDuplicates;
        }
    }
}
