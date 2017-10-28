namespace Database.Entities
{
    public class AccountEntity : BaseEntity
    {
        public string EmailAddress { get; set; }
        
        public string Password { get; set; }
    }
}
