﻿using Core.Common;

namespace Core.Entities
{
    public class Bill : BaseEntity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public Guid OrderNo { get; set; }
        public string WorkpointName { get; set; }
        public string CompanyName { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
