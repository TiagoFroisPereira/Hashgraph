﻿using Hashgraph.Implementation;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph
{
    public partial class Client
    {
        /// <summary>
        /// Suspends the associated account's ability to send or
        /// receive the specified token.
        /// </summary>
        /// <param name="token">
        /// The identifier (Address/Symbol) of the token to suspend/freeze.
        /// </param>
        /// <param name="address">
        /// Address of the account to suspend.
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
        public async Task<TransactionReceipt> SuspendTokenAsync(Address token, Address address, Action<IContext>? configure = null)
        {
            return new TransactionReceipt(await SuspendTokenImplementationAsync(token, address, null, configure, false));
        }
        /// <summary>
        /// Suspends the associated account's ability to send or
        /// receive the specified token.
        /// </summary>
        /// <param name="token">
        /// The identifier (Address/Symbol) of the token to suspend/freeze.
        /// </param>
        /// <param name="address">
        /// Address of the account to suspend.
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
        public async Task<TransactionReceipt> SuspendTokenAsync(Address token, Address address, Signatory signatory, Action<IContext>? configure = null)
        {
            return new TransactionReceipt(await SuspendTokenImplementationAsync(token, address, signatory, configure, false));
        }
        /// <summary>
        /// Suspends the associated account's ability to send or
        /// receive the specified token.
        /// </summary>
        /// <param name="token">
        /// The identifier (Address/Symbol) of the token to suspend/freeze.
        /// </param>
        /// <param name="address">
        /// Address of the account to suspend.
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
        public async Task<TransactionRecord> SuspendTokenWithRecordAsync(Address token, Address address, Action<IContext>? configure = null)
        {
            return new TransactionRecord(await SuspendTokenImplementationAsync(token, address, null, configure, true));
        }
        /// <summary>
        /// Suspends the associated account's ability to send or
        /// receive the specified token.
        /// </summary>
        /// <param name="token">
        /// The identifier (Address/Symbol) of the token to suspend/freeze.
        /// </param>
        /// <param name="address">
        /// Address of the account to suspend.
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
        public async Task<TransactionRecord> SuspendTokenWithRecordAsync(Address token, Address address, Signatory signatory, Action<IContext>? configure = null)
        {
            return new TransactionRecord(await SuspendTokenImplementationAsync(token, address, signatory, configure, true));
        }
        /// <summary>
        /// Internal implementation of delete token method.
        /// </summary>
        private async Task<NetworkResult> SuspendTokenImplementationAsync(Address token, Address address, Signatory? signatory, Action<IContext>? configure, bool includeRecord)
        {
            token = RequireInputParameter.Token(token);
            address = RequireInputParameter.Address(address);
            await using var context = CreateChildContext(configure);
            var transactionBody = new TransactionBody
            {
                TokenFreeze = new TokenFreezeAccountTransactionBody
                {
                    Token = new TokenID(token),
                    Account = new AccountID(address)
                }
            };
            return await transactionBody.SignAndExecuteWithRetryAsync(context, includeRecord, "Unable to Suspend Token, status: {0}", signatory);
        }
    }
}
