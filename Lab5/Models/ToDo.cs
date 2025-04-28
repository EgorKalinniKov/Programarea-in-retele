using System.ComponentModel.DataAnnotations;

namespace Lab5.Models
{
    public class ToDo
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = String.Empty;


        [DataType(DataType.MultilineText)]
        public string Details { get; set; } = String.Empty;

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}
