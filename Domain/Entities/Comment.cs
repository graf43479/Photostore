using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }

        [Required(ErrorMessage = "Введите ваше имя")]
        public string Tittle { get; set; }

        [Required(ErrorMessage = "Введите текст отзыва")]
        [MinLength(15, ErrorMessage = "Длинна текста слишком маленькая")]
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsAccept { get; set; }

    }
}
