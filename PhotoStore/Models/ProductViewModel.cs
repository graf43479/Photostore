using System;
using System.Collections.Generic;
using Domain.Entities;

namespace PhotoStore.Models
{
    public class ProductViewModel
    {

        public int ProductID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Sequence { get; set; }
        public string ImgExt { get; set; }
        public string Path { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsChoosen { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string CategoryName { get; set; }
        public int SelectedCategoryID { get; set; }
    }
}

