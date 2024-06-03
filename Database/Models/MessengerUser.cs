using System.ComponentModel.DataAnnotations;


namespace dotnet_messenger.Database.Models
{
    public class MessengerUser
    {
        [Key]
        public int Id { get; set; }
		public required string Login { get; set; }
		public required string Password { get; set; }
		public required string Username { get; set; }
	}
}
