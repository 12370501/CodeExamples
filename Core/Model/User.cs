using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using CustomExtensions.DataAnnotations;

namespace Ebuy
{
    [MetadataType(typeof(User.Metadata))]
    public class User : Entity
    {
        public string DisplayName
        {
            get { return _displayName ?? FullName; }
            set { _displayName = value; }
        }
        private string _displayName;

        [Unique]
        public string EmailAddress { get; set; }

        public string FullName { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Auction> WatchedAuctions { get; set; }

        protected override string GenerateKey()
        {
            if(string.IsNullOrWhiteSpace(DisplayName))
                // TODO: Localize
                throw new EntityKeyGenerationException(GetType(), "DisplayName is empty");

            return KeyGenerator.Generate(DisplayName);
        }


        public void Bid(Auction auction, Currency bidAmount)
        {
            Bid(auction, bidAmount, DateTime.Now);
        }

        protected internal virtual void Bid(Auction auction, Currency bidAmount, DateTime timestamp)
        {
            Contract.Requires(auction != null);
            Contract.Requires(bidAmount != null);

            auction.PostBid(this, bidAmount, timestamp);
        }


        public class Metadata
        {
            [StringLength(50)]
            public object DisplayName { get; set; }

            [StringLength(100, MinimumLength = 5)]
            public object EmailAddress { get; set; }

            [Required, StringLength(100, MinimumLength = 3)]
            public object FullName { get; set; }
        }
    }
}