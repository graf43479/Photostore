using System;

namespace PhotoStore.Models
{
    public class CategoryViewModel 
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ShortName { get; set; }
        public int Sequence { get; set; }
        public string KeyWords { get; set; }
        public string Snippet { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }

        public int PhotoCount { get; set; }

        public int ChoosenCount { get; set; }

        public int VisibleCount { get; set; }

    }
}

/*
    [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ShortName { get; set; }
        public int Sequence { get; set; }
        public string KeyWords { get; set; }
        public string Snippet { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Product> Products { get; set; }
 */