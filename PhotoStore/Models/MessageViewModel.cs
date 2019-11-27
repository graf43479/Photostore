using System.ComponentModel.DataAnnotations;

namespace PhotoStore.Models
{
    public class MessageViewModel
    {

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле Email")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9.-]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Сообщение")]
        public string Text { get; set; }
    }
}