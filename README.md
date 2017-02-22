# OurGroceries.Api
Integration API for OurGroceries.com written in C# .NET

# Usage

```c#
using (OurGroceries.Api.Client client = new OurGroceries.Api.Client())
{
    var emailAddress = "YOUR EMAIL ADDRESS";
    var password = "YOUR PASSWORD";
    
    var shoppingTeamJson = client.SignIn(emailAddress, password);
    
    var shoppingList = string.Empty;

    if(!string.IsNullOrEmpty(shoppingTeamJson))
    {
        var shoppingTeam = Newtonsoft.Json.JsonConvert.DeserializeObject<OurGroceries.Api.Entities.ShoppingTeam>(shoppingTeamJson);

        if (shoppingTeam == null || shoppingTeam.shoppingLists.Count == 0) throw new System.Exception("No list returned.");

        shoppingList = client.GetList(listId: shoppingTeam.shoppingLists[0].id, teamId: shoppingTeam.teamId);
    }
}
```
