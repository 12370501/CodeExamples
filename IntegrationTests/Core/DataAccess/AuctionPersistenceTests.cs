using System.Linq;
using Ebuy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Core.DataAccess
{
    [TestClass]
    public class AuctionPersistenceTests : RepositoryTestFixture
    {
        [TestMethod]
        public void ShouldSaveNewAuction()
        {
            AssertCanSaveNewEntity<Auction>();
        }

        [TestMethod]
        public void ShouldFindAuctionById()
        {
            AssertCanFindByKey<Auction>();
        }

        [TestMethod]
        public void ShouldPersistBids()
        {
            var auction = CreateAndSaveNewEntity<Auction>();
            var user1 = CreateAndSaveNewEntity<User>();
            var user2 = CreateAndSaveNewEntity<User>();

            auction.PostBid(user1, "$10");
            auction.PostBid(user2, "$20");
            auction.PostBid(user1, "$30");

            DataContext.SaveChanges();

            ExecuteInNewContext(context => {
                var savedAuction = context.Auctions.Find(auction.Id);
                Assert.AreEqual(3, savedAuction.Bids.Count);
                Assert.AreEqual((Currency)"$30", savedAuction.WinningBid.Amount);

                var savedUser = context.Users.Find(user1.Id);
                Assert.AreEqual(2, savedUser.Bids.Count);
                Assert.IsTrue(savedUser.Bids.OrderBy(x => x.Amount.Value).Last().IsWinningBid);
            });
        }
    }
}