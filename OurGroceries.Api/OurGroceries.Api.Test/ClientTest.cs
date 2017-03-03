using Microsoft.VisualStudio.TestTools.UnitTesting;
using OurGroceries.Api;
using OurGroceries.Api.Entities;
using Newtonsoft.Json;

namespace OurGroceries.Test
{
    [TestClass]
    public class ClientTest
    {
        const string emailAddress = "** Your Emailaddress **";
        const string password = "** Your Password **";

        [TestMethod]
        public void Signup_HappyPath()
        {
            using (Client client = new Client())
            {
                var actual = client.SignIn(emailAddress, password);

                Assert.IsTrue(!string.IsNullOrEmpty(actual));
            }            
        }

        [TestMethod]
        public void GetList_HappyPath()
        {
            using (Client client = new Client())
            {
                var shoppingTeamJson = client.SignIn(emailAddress, password);
                var actual = string.Empty;

                if(!string.IsNullOrEmpty(shoppingTeamJson))
                {
                    var shoppingTeam = JsonConvert.DeserializeObject<ShoppingTeam>(shoppingTeamJson);

                    if (shoppingTeam == null || shoppingTeam.shoppingLists.Count == 0) Assert.Fail();

                    actual = client.GetList(listId: shoppingTeam.shoppingLists[0].id, teamId: shoppingTeam.teamId);
                }

                Assert.IsFalse(string.IsNullOrEmpty(actual));
            }
        }

        [TestMethod]
        public void UpdateCrossedOffItem_HappyPath()
        {
            using (Client client = new Client())
            {
                var shoppingTeamJson = client.SignIn(emailAddress, password);
                var actual = string.Empty;

                if (!string.IsNullOrEmpty(shoppingTeamJson))
                {
                    var shoppingTeam = JsonConvert.DeserializeObject<ShoppingTeam>(shoppingTeamJson);

                    if (shoppingTeam == null || shoppingTeam.shoppingLists.Count == 0) Assert.Fail();

                    actual = client.UpdateCrossedOffItem(listId: shoppingTeam.shoppingLists[0].id, teamId: shoppingTeam.teamId, itemId:"", crossedOff: true);
                }

                Assert.IsFalse(string.IsNullOrEmpty(actual));
            }
        }
    }
}
