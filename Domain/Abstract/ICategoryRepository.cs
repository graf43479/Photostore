using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }

        Category GetCategoryByShortName(string shortName);

        void SaveCategory(Category category);

        void DeleteCategory(Category category);

        string GetShortName(string name, int maxID);

        void RefreshAllShortNames();

        void UpdateCategorySequence();

        void CategorySequence(int categoryId, string actionType);

        //void SetDeletedStatus(bool isDeleted, Category category);

    }
}
