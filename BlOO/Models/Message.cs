using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }

        [Required]
        [ForeignKey(nameof(Receiver))]
        public int ReceiverId { get; set; } 


        [Required]
        public string Content {  get; set; }

        public bool MessageSeen { get; set; } = false;

        public DateTime SendingDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [InverseProperty("SentMessages")]
        public virtual ApplicationUser Sender { get; set; }

        [InverseProperty("ReceivedMessages")]
        public virtual ApplicationUser Receiver { get; set; }
    }
}
