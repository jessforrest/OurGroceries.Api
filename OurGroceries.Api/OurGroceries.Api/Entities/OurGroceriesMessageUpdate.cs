namespace OurGroceries.Api.Entities
{
    public class OurGroceriesMessageUpdate : OurGroceriesMessage
    {
        public string itemId { get; set;}
        public bool crossedOff { get; set;}
    }
}
