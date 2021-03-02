﻿using Hashgraph.Implementation;
using Proto;
using System;
using System.Threading.Tasks;

namespace Hashgraph
{
    public partial class Client
    {
        /// <summary>
        /// Removes a file from the network via Administrative Delete
        /// </summary>
        /// <param name="fileToDelete">
        /// The address of the file to delete.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction receipt indicating success of the file deletion.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public async Task<TransactionReceipt> SystemDeleteFileAsync(Address fileToDelete, Action<IContext>? configure = null)
        {
            return new TransactionReceipt(await SystemDeleteFileImplementationAsync(fileToDelete, null, configure, false));
        }
        /// <summary>
        /// Removes a file from the network via Administrative Delete
        /// </summary>
        /// <param name="fileToDelete">
        /// The address of the file to delete.
        /// </param>
        /// <param name="signatory">
        /// Typically private key, keys or signing callback method that 
        /// are needed to delete the file as per the requirements in the
        /// associated Endorsement.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction receipt indicating success of the file deletion.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public async Task<TransactionReceipt> SystemDeleteFileAsync(Address fileToDelete, Signatory signatory, Action<IContext>? configure = null)
        {
            return new TransactionReceipt(await SystemDeleteFileImplementationAsync(fileToDelete, signatory, configure, false));
        }
        /// <summary>
        /// Removes a file from the network via Administrative Delete
        /// </summary>
        /// <param name="fileToDelete">
        /// The address of the file to delete.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction record indicating success of the file deletion,
        /// fees & other transaction details.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public async Task<TransactionRecord> SystemDeleteFileWithRecordAsync(Address fileToDelete, Action<IContext>? configure = null)
        {
            return new TransactionRecord(await SystemDeleteFileImplementationAsync(fileToDelete, null, configure, true));
        }
        /// <summary>
        /// Removes a file from the network via Administrative Delete
        /// </summary>
        /// <param name="fileToDelete">
        /// The address of the file to delete.
        /// </param>
        /// <param name="signatory">
        /// Typically private key, keys or signing callback method that 
        /// are needed to delete the file as per the requirements in the
        /// associated Endorsement.
        /// </param>
        /// <param name="configure">
        /// Optional callback method providing an opportunity to modify 
        /// the execution configuration for just this method call. 
        /// It is executed prior to submitting the request to the network.
        /// </param>
        /// <returns>
        /// A transaction record indicating success of the file deletion,
        /// fees & other transaction details.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If required arguments are missing.</exception>
        /// <exception cref="InvalidOperationException">If required context configuration is missing.</exception>
        /// <exception cref="PrecheckException">If the gateway node create rejected the request upon submission.</exception>
        /// <exception cref="ConsensusException">If the network was unable to come to consensus before the duration of the transaction expired.</exception>
        /// <exception cref="TransactionException">If the network rejected the create request as invalid or had missing data.</exception>
        public async Task<TransactionRecord> SystemDeleteFileWithRecordAsync(Address fileToDelete, Signatory signatory, Action<IContext>? configure = null)
        {
            return new TransactionRecord(await SystemDeleteFileImplementationAsync(fileToDelete, signatory, configure, true));
        }
        /// <summary>
        /// Internal helper function implementing the file delete functionality.
        /// </summary>
        private async Task<NetworkResult> SystemDeleteFileImplementationAsync(Address fileToDelete, Signatory? signatory, Action<IContext>? configure, bool includeRecord)
        {
            fileToDelete = RequireInputParameter.FileToDelete(fileToDelete);
            await using var context = CreateChildContext(configure);
            var transactionBody = new TransactionBody
            {
                SystemDelete = new SystemDeleteTransactionBody
                {
                    FileID = new FileID(fileToDelete)
                }
            };
            return await transactionBody.SignAndExecuteWithRetryAsync(context, includeRecord, "Unable to delete file, status: {receipt.Status}", signatory);
        }
    }
}
