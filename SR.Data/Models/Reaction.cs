namespace SR.Data.Models
{
    public class Reaction : EntityBase
    {
        public Enums.Reaction Type { get; set; }
        public string Url { get; set; }
    }
}
