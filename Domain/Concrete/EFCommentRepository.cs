using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFCommentRepository : ICommentRepository
    {
         private PhotoDBContext context;

         public EFCommentRepository(PhotoDBContext context)
        {
            this.context = context;
        }

         public IQueryable<Comment> Comments
         {
             get { return context.Comments; } 
            }

      public void SaveComment(Comment comment)
        {
            if (comment.CommentID == 0)
            {
                comment.CreateDate = DateTime.Now;

                context.Comments.Add(comment);
            }
            else
            {
                //Comment com = context.Comments.FirstOrDefault(x => x.CommentID == comment.CommentID);
                context.Entry(comment).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            context.Comments.Remove(comment);
            context.SaveChanges();
        }
    }
}
