using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public required string SenderUsername { get; set; }
        public required string RecipientUsername { get; set; }
        public required string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        //navigation properties
        public int SenderId { get; set; }
        public AppUser Sender { get; set; } = null!;
        public int RecipientId { get; set; }
        public AppUser Recipient { get; set; } = null!;

    }
}