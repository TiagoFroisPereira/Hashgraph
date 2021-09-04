﻿using Grpc.Core;
using Hashgraph;
using Hashgraph.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Proto
{
    public sealed partial class TokenFeeScheduleUpdateTransactionBody : INetworkTransaction
    {
        string INetworkTransaction.TransactionExceptionMessage => "Unable to Update Royalties, status: {0}";

        SchedulableTransactionBody INetworkTransaction.CreateSchedulableTransactionBody()
        {
            throw new InvalidOperationException("Updating Token Royalties is not a schedulable transaction.");
        }

        TransactionBody INetworkTransaction.CreateTransactionBody()
        {
            return new TransactionBody { TokenFeeScheduleUpdate = this };
        }

        Func<Transaction, Metadata?, DateTime?, CancellationToken, AsyncUnaryCall<TransactionResponse>> INetworkTransaction.InstantiateNetworkRequestMethod(Channel channel)
        {
            return new TokenService.TokenServiceClient(channel).updateTokenFeeScheduleAsync;
        }

        internal TokenFeeScheduleUpdateTransactionBody(Address token, IEnumerable<IRoyalty>? royalties) : this()
        {
            TokenId = new TokenID(token);
            // Note: Null & Empty are Valid, they will clear the list of fees.
            if (royalties is not null)
            {
                CustomFees.AddRange(royalties.Select(royalty => new CustomFee(royalty)));
            }
        }
    }
}
