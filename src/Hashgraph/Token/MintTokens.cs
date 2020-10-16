﻿using Grpc.Core;
using Hashgraph.Implementation;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph
{
    public partial class Client
    {
        /// <summary>
        /// Adds token coins to the treasury.
        /// </summary>
        /// <param name="token">
        /// The identifier (Address/Symbol) of the token to add coins to.
        /// </param>
        /// <param name="amount">
        /// The amount of coins to add (of the divisible denomination)
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction receipt indicating a successful operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission, for example of the token is already deleted.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> MintTokenAsync(Address token, ulong amount, Action<IContext>? configure = null)
        {
            return MintTokenImplementationAsync<TransactionReceipt>(token, amount, null, configure);
        }
        /// <summary>
        /// Adds token coins to the treasury.
        /// </summary>
        /// <param name="token">
        /// The identifier (Address/Symbol) of the token to add coins to.
        /// </param>
        /// <param name="amount">
        /// The amount of coins to add (of the divisible denomination)
        /// </param>
        /// <param name="signatory">
        /// Additional signing key matching the administrative endorsements
        /// associated with this token (if not already added in the context).
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction receipt indicating a successful operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission, for example of the token is already deleted.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionReceipt> MintTokenAsync(Address token, ulong amount, Signatory signatory, Action<IContext>? configure = null)
        {
            return MintTokenImplementationAsync<TransactionReceipt>(token, amount, signatory, configure);
        }
        /// <summary>
        /// Adds token coins to the treasury.
        /// </summary>
        /// <param name="token">
        /// The identifier (Address/Symbol) of the token to add coins to.
        /// </param>
        /// <param name="amount">
        /// The amount of coins to add (of the divisible denomination)
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction record indicating a successful operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission, for example of the token is already deleted.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> MintTokenWithRecordAsync(Address token, ulong amount, Action<IContext>? configure = null)
        {
            return MintTokenImplementationAsync<TransactionRecord>(token, amount, null, configure);
        }
        /// <summary>
        /// Adds token coins to the treasury.
        /// </summary>
        /// <param name="token">
        /// The identifier (Address/Symbol) of the token to add coins to.
        /// </param>
        /// <param name="amount">
        /// The amount of coins to add (of the divisible denomination)
        /// </param>
        /// <param name="signatory">
        /// Additional signing key matching the administrative endorsements
        /// associated with this token (if not already added in the context).
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction record indicating a successful operation.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission, for example of the token is already deleted.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public Task<TransactionRecord> MintTokenWithRecordAsync(Address token, ulong amount, Signatory signatory, Action<IContext>? configure = null)
        {
            return MintTokenImplementationAsync<TransactionRecord>(token, amount, signatory, configure);
        }
        /// <summary>
        /// Internal implementation of mint token method.
        /// </summary>
        private async Task<TResult> MintTokenImplementationAsync<TResult>(Address token, ulong amount, Signatory? signatory, Action<IContext>? configure) where TResult : new()
        {
            token = RequireInputParameter.Token(token);
            amount = RequireInputParameter.TokenAmount(amount);
            await using var context = CreateChildContext(configure);
            RequireInContext.Gateway(context);
            var payer = RequireInContext.Payer(context);
            var signatories = Transactions.GatherSignatories(context, signatory);
            var transactionId = Transactions.GetOrCreateTransactionID(context);
            var transactionBody = Transactions.CreateTransactionBody(context, transactionId);
            transactionBody.TokenMint = new TokenMintTransactionBody
            {
                Token = new TokenID(token),
                Amount = amount
            };
            var request = await Transactions.SignTransactionAsync(transactionBody, signatories);
            var precheck = await Transactions.ExecuteSignedRequestWithRetryAsync(context, request, getRequestMethod, getResponseCode);
            ValidateResult.PreCheck(transactionId, precheck);
            var receipt = await GetReceiptAsync(context, transactionId);
            if (receipt.Status != ResponseCodeEnum.Success)
            {
                throw new TransactionException($"Unable to Mint Token Coins, status: {receipt.Status}", transactionId.ToTxId(), (ResponseCode)receipt.Status);
            }
            var result = new TResult();
            if (result is TransactionRecord rec)
            {
                var record = await GetTransactionRecordAsync(context, transactionId);
                record.FillProperties(rec);
            }
            else if (result is TransactionReceipt rcpt)
            {
                receipt.FillProperties(transactionId, rcpt);
            }
            return result;

            static Func<Transaction, Task<TransactionResponse>> getRequestMethod(Channel channel)
            {
                var client = new TokenService.TokenServiceClient(channel);
                return async (Transaction transaction) => await client.mintTokenAsync(transaction);
            }

            static ResponseCodeEnum getResponseCode(TransactionResponse response)
            {
                return response.NodeTransactionPrecheckCode;
            }
        }
    }
}
