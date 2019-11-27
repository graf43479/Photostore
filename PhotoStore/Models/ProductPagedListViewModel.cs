using System.Collections.Generic;
using Domain.Entities;

namespace PhotoStore.Models
{
    public class ProductPagedListViewModel
    {
            public IEnumerable<Product> Products { get; set; }
            public PagingInfo PagingInfo { get; set; }
            public string CurrentCategory { get; set; }
            public string Description { get; set; }
            public string Snippet { get; set; }
    }
}