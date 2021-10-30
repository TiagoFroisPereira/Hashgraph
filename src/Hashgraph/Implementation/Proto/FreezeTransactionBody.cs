﻿using Google.Protobuf;
using Grpc.Core;
using Hashgraph;
using Hashgraph.Implementation;
using System;
using System.Threading;

namespace Proto
{
    public sealed partial class FreezeTransactionBody : INetworkTransaction
    {
        string INetworkTransaction.TransactionExceptionMessage => "Failed to submit suspend/freeze command, status: {0}";

        SchedulableTransactionBody INetworkTransaction.CreateSchedulableTransactionBody()
        {
            return new SchedulableTransactionBody { Freeze = this };
        }

        TransactionBody INetworkTransaction.CreateTransactionBody()
        {
            return new TransactionBody { Freeze = this };
        }

        Func<Transaction, Metadata?, DateTime?, CancellationToken, AsyncUnaryCall<TransactionResponse>> INetworkTransaction.InstantiateNetworkRequestMethod(Channel channel)
        {
            return new FreezeService.FreezeServiceClient(channel).freezeAsync;
        }

        internal FreezeTransactionBody(FreezeType freezeType) : this()
        {
            FreezeType = freezeType;
        }

        internal FreezeTransactionBody(DateTime consensusTime, FreezeType freezeType) : this()
        {
            StartTime = new Timestamp(consensusTime);
            FreezeType = freezeType;
        }

        internal FreezeTransactionBody(Address fileAddress, ReadOnlyMemory<byte> fileHash) : this()
        {
            if (fileAddress.IsNullOrNone())
            {
                throw new ArgumentOutOfRangeException(nameof(fileAddress), "The upgrade file's File Address ID is missing.");
            }
            if (fileHash.IsEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(fileHash), "The hash of the file contents must be included.");
            }
            UpdateFile = new FileID(fileAddress);
            FileHash = ByteString.CopyFrom(fileHash.Span);
            FreezeType = FreezeType.PrepareUpgrade;
        }
    }
}
