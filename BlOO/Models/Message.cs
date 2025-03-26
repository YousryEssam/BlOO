using System.ComponentModel.DataAnnotations;

namespace BlOO.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId {  get; set; }//un completed
        public int RecipientId {  get; set; }//un completed

        [Required]
        public string Content {  get; set; }

        public bool MessageSeen {  get; set; }=false;

        public DateTime SendingDate { get; set; } = DateTime.UtcNow;

    }
}
