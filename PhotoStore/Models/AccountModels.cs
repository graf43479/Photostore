using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PhotoStore.Models
{
    public class LoginModel
    {
        
        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "* Необходимо корректно заполнить поле \"Логин\"")]
        public string UserName { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "* Необходимо корректно заполнить поле \"Пароль\"")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }


        [Display(Name = "Докажите, что вы не робот")]
        public string Captcha { get; set; }
    }

    public class RegisterModel
    {
        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Необходимо корректно заполнить поле Логин")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Необходимо корректно заполнить поле Пароль")]
        [DataType(DataType.Password)]
        [MinLength(7, ErrorMessage = "Длина пароля должна быть более 6 символов")]
        public string Password { get; set; }

        // [Required(ErrorMessage = "Длина пароля должна быть более 6 символов")]
        [MinLength(7, ErrorMessage = "Длина пароля должна быть более 6 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Повтор пароля")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль подтвержден неверно")]
        public string ConfirmPassword { get; set; }

        //  [Required(ErrorMessage = "Старый пароль указан неверно!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        [Required(ErrorMessage = "Необходимо заполнить поле Email")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9.-]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; }


        public DateTime Created { get; set; }

        public string PasswordSalt { get; set; }
        [DefaultValue(false)]
        public bool IsActivated { get; set; }
    }

    
}
