using System.Collections.Generic;

namespace OurGroceries.Api.Entities
{
    public class ShoppingTeam
    {
        public string teamId { get; set; }

        public List<ShoppingList> shoppingLists { get; set; }
    }
}
