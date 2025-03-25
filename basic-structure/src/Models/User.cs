using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace src.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}