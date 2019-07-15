﻿using Hashgraph.Implementation;
using System;

namespace Hashgraph
{
    /// <summary>
    /// Represents the transaction id associated with a network request.  
    /// This is generated by the library automatically for each request.  
    /// <code>TxId</code> implements the equitable interface and can be 
    /// compared to other transaction ids returned from the library.
    /// </summary>
    public sealed class TxId : IEquatable<TxId>
    {
        /// <summary>
        /// The address of the account paying the
        /// transaction processing fee.
        /// </summary>
        public Address Address { get; internal set; }
        /// <summary>
        /// The number of whole seconds since the Epoch.
        /// </summary>
        public long ValidStartSeconds { get; internal set; }
        /// <summary>
        /// The number of nanoseconds added to the 
        /// <see cref="ValidStartSeconds"/> value to 
        /// produce the total amount of time since the
        /// Epoch.
        /// </summary>
        /// <remarks>
        /// Unfortunately the native .net DateTime class
        /// does not represent time at the resolution of
        /// nano-seconds.  Therefore it is necessary to
        /// represent the date time in this manner.
        /// </remarks>
        public int ValidStartNanos { get; internal set; }
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="address">
        /// Address associated with 
        /// and paying for this transaction.
        /// </param>
        /// <param name="seconds">
        /// The total number of seconds elapsed since
        /// the Epoch.
        /// </param>
        /// <param name="nanos">
        /// Total number of nanoseconds elapsed past the
        /// seconds past the Epoch.
        /// </param>
        public TxId(Address address, long seconds, int nanos)
        {
            Address = address;
            ValidStartSeconds = seconds;
            ValidStartNanos = nanos;
        }
        /// <summary>
        /// Convenience constructor converting the
        /// entered <code>DateTime</code> object into
        /// the proper seconds and nanoseconds since the
        /// Epoch for use by the network.
        /// </summary>
        /// <param name="address">
        /// Address associated with 
        /// and paying for this transaction.
        /// </param>
        /// <param name="dateTime">
        /// The Date & Time stamp associated with the
        /// transaction.
        /// </param>
        public TxId(Address address, DateTime dateTime)
        {
            Address = address;
            (ValidStartSeconds, ValidStartNanos) = Epoch.FromDate(dateTime);
        }
        /// <summary>
        /// Internal Constructor, for now limit creation
        /// of the uninitialized TxId to the library itself.
        /// </summary>
        internal TxId()
        {
            // Because we don't want to set
            // this property to nullable
            Address = new Address(0, 0, 0);
        }
        /// <summary>
        /// Equality implementation.
        /// </summary>
        /// <param name="other">
        /// The other <code>TxId</code> object to compare.
        /// </param>
        /// <returns>
        /// True if the id is identical to the 
        /// other <code>TxId</code> object.
        /// </returns>
        public bool Equals(TxId other)
        {
            if (other is null)
            {
                return false;
            }
            return
                ValidStartNanos == other.ValidStartNanos &&
                ValidStartSeconds == other.ValidStartSeconds &&
                Address == other.Address;
        }
        /// <summary>
        /// Equality implementation.
        /// </summary>
        /// <param name="obj">
        /// The other <code>TxId</code> object to compare (if it is
        /// an <code>TxId</code>).
        /// </param>
        /// <returns>
        /// If the other object is an TxId, then <code>True</code> 
        /// if id is identical to the other <code>TxId</code> object, 
        /// otherwise <code>False</code>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj is TxId other)
            {
                return Equals(other);
            }
            return false;
        }
        /// <summary>
        /// Equality implementation.
        /// </summary>
        /// <returns>
        /// A unique hash of the contents of this <code>TxId</code> 
        /// object.  Only consistent within the current instance of 
        /// the application process.
        /// </returns>
        public override int GetHashCode()
        {
            return $"TxID:{Address.GetHashCode()}:{ValidStartSeconds}:{ValidStartNanos}".GetHashCode();
        }
        /// <summary>
        /// Equals implementation.
        /// </summary>
        /// <param name="left">
        /// Left hand <code>TxId</code> argument.
        /// </param>
        /// <param name="right">
        /// Right hand <code>TxId</code> argument.
        /// </param>
        /// <returns>
        /// <code>True</code> id is identical within each <code>TxId</code> objects.
        /// </returns>
        public static bool operator ==(TxId left, TxId right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }
        /// <summary>
        /// Not equals implementation.
        /// </summary>
        /// <param name="left">
        /// Left hand <code>TxId</code> argument.
        /// </param>
        /// <param name="right">
        /// Right hand <code>TxId</code> argument.
        /// </param>
        /// <returns>
        /// <code>False</code> if the id is identical within 
        /// each <code>TxId</code> object.  <code>True</code> 
        /// if they are not identical.
        /// </returns>
        public static bool operator !=(TxId left, TxId right)
        {
            return !(left == right);
        }
    }
}
