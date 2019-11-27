using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Entities
{
    public class Role
    {
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название роли")]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }



    }
}