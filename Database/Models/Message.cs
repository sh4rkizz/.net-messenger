using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dotnet_messenger.Database.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int FromId { get; set; }
        public MessengerUser? From { get; set; }
        public int ToId { get; set; }
        public MessengerUser? To { get; set; }
        public string? Title { get; set; }

        public string? Text { get; set; }
        public DateTime Date { get; set; }

        public bool Status { get; set; }
    }
}
