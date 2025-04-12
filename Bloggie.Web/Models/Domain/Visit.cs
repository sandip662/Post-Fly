namespace Bloggie.Web.Models.Domain
{
    public class Visit
    {
        public Guid Id { get; set; }
        public DateTime VisitedAt { get; set; }
        public string? IPAddress { get; set; }
    }

}
