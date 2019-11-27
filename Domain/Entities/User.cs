using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }

        public int RoleID { get; set; }
        
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(7, ErrorMessage = "Длинна пароля должна быть более 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9.-]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; }

        
        
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        
        public string PasswordSalt { get; set; }
        
        [DefaultValue(false)]
        public bool IsActivated { get; set; }

        public string NewEmailKey { get; set; }

        public virtual Role Role { get; set; }

        
    }
}