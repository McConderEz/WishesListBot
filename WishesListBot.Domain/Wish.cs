namespace WishesListBot.Domain
{
    public class Wish
    {
        public int Id { get; set; }
        public string Description {  get; set; }
        public DateTime DateTime { get; set; }
        public string RecipientName { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
