using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoStore.Models
{
    public class ProductAPI
    {
        public int ProductID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Sequence { get; set; }
        public string ImgExt { get; set; }
        public string Path { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsChoosen { get; set; }
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
    }
}