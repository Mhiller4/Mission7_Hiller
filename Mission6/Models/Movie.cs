using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mission6.Models
{
    public class Movie // Creates class for Movies and contains all parameters we're going to pass through
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }

        // Navigation property; keeping the name as in your file
        public Category Category { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1888, 2100, ErrorMessage = "Enter a valid year.")]
        public int Year { get; set; }
        
        public string? Director { get; set; }

        [Required(ErrorMessage = "Edited is required.")]
        public bool Edited { get; set; }

        [Required]
        public string Rating { get; set; } // Dropdown selection (G, PG, PG-13, R)

        public string? LentTo { get; set; } // Optional

        [Required(ErrorMessage = "CopiedToPlex is required.")]
        public bool CopiedToPlex { get; set; }

        [MaxLength(25, ErrorMessage = "Notes must be 25 characters or fewer.")]
        public string? Notes { get; set; } // Optional with length restriction
    }
}