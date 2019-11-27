using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFCategoryRepository :  ICategoryRepository
    {
        private PhotoDBContext context;

        public EFCategoryRepository(PhotoDBContext context)
        {
            this.context = context;
        }

        public IQueryable<Category> Categories {
            get { return context.Categories.Include("Products"); } 
            }
        

        public Category GetCategoryByShortName(string shortName)
        {
            try
            {
                Category category = context.Categories.FirstOrDefault(x => x.ShortName == shortName);
                return category;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SaveCategory(Category category)
        {
            if (category.CategoryID == 0)
            {

                try
                {
                    category.Sequence = context.Categories.Select(x => x.Sequence).Max() + 1;
                }
                catch (Exception)
                {
                    category.Sequence = 1;
                }

                if (category.ShortName == null)

                    category.ShortName = GetShortName(category.CategoryName, context.Categories.Max(x => x.CategoryID) + 1);

                category.UpdateDate = DateTime.Now;

                context.Categories.Add(category);
            }
            else
            {
                Category changingCat = context.Categories.FirstOrDefault(x => x.CategoryID == category.CategoryID);
                
                changingCat.CategoryID = category.CategoryID;
                changingCat.CategoryName = category.CategoryName;
                changingCat.Sequence = category.Sequence;
                changingCat.ShortName = category.ShortName;
                changingCat.KeyWords = category.KeyWords;
                changingCat.Snippet = category.Snippet;
                changingCat.Description = category.Description;
                changingCat.UpdateDate = DateTime.Now;
                changingCat.IsActive = category.IsActive;
                changingCat.CreateDate = category.CreateDate;

                context.Entry(changingCat).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            try
            {
                foreach (var c in context.Categories)
                {
                    if (c.Sequence < category.Sequence)
                    {
                    }
                    else
                    {
                        --c.Sequence;
                        context.Entry(c).State = EntityState.Modified;
                       // context.SaveChanges();
                    }
                }

            }
            //catch (OptimisticConcurrencyException)
            //{
            //    if (ObjectStateManager.GetObjectStateEntry(entity).State == EntityState.Deleted || ObjectStateManager.GetObjectStateEntry(entity).State == EntityState.Modified)
            //        this.Refresh(RefreshMode.StoreWins, entity);
            //    else if (ObjectStateManager.GetObjectStateEntry(entity).State == EntityState.Added)
            //        Detach(entity);
            //    AcceptAllChanges();
            //    transaction.Commit();
            //}
            catch (Exception ex)
            {
                var extes = ex.Message;
            }
            finally
            {
                //context.Categories.
                context.Categories.Remove(category);
                context.SaveChanges();
            }

        }

        public string GetShortName(string name, int maxID)
        {
            var c = context.Categories.ToList();
            string s = Constants.TransliterateText(name);
            if (c.Any(x => x.ShortName == s && x.CategoryID != maxID))
            {
                return s + maxID.ToString();
            }
            return s;
        }

        public void RefreshAllShortNames()
        {
            var c = context.Categories.ToList();

            for (int i = 0; i < 2; i++)
            {
                foreach (var category in c)
                {
                    category.ShortName = GetShortName(category.CategoryName, category.CategoryID);
                    context.Entry(category).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public void UpdateCategorySequence()
        {
            int i = 1;
            foreach (var category in Categories)
            {
                category.Sequence = i;
                context.Entry(category).State = EntityState.Modified;
                i++;
            }
            context.SaveChanges();
        }

        public void CategorySequence(int categoryId, string actionType)
        {
            Category category1 =
              context.Categories.FirstOrDefault(x => x.CategoryID == categoryId);

            if (actionType == "Up")
            {
                if (category1.Sequence == 1)
                {
                }
                else
                {
                    Category category2 =
                    context.Categories.FirstOrDefault(x => x.Sequence == category1.Sequence - 1);
                    category2.Sequence = category2.Sequence + 1;
                    category1.Sequence = category1.Sequence - 1;
                    context.Entry(category1).State = EntityState.Modified;
                    context.Entry(category2).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            else if (actionType == "Down")
            {
                if (category1.Sequence == context.Categories.Max(x => x.Sequence))
                {
                    return;
                }
                else
                {
                    Category category2 =
                context.Categories.FirstOrDefault(x => x.Sequence == category1.Sequence + 1);
                    category2.Sequence = category2.Sequence - 1;
                    category1.Sequence = category1.Sequence + 1;
                    context.Entry(category1).State = EntityState.Modified;
                    context.Entry(category2).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
    }
}
