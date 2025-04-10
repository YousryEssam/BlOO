namespace BlOO.ViewModels
{
    public class ReportPostWithReporterNameVM
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Name { get; set; }
        public ReportReason Content { get; set; }

        public string ImgUrl {get; set;}
    }
}
