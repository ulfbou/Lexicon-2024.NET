﻿using Azure.Data.Tables;

namespace TenantManagement.Entities
{
    public class Announcement : TenantEntity, ITableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Course Course { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}