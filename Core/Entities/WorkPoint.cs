namespace Core.Entities
{
    public class WorkPoint : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int CreatedBy { get; set; }
        public int CompanyId { get; set; } 
        public virtual Company Company { get; set; }
    }
}
