using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Sequence { get; set; }
        public string ImgExt { get; set; }
        public string Path { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsChoosen { get; set; }
        public virtual Category Category { get; set; }
    }
}