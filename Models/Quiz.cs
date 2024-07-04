﻿using Microsoft.AspNetCore.Identity;

namespace Dydaktycznie.Models
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<QuizQuestion>?QuizQuestions { get; set; }
        public byte[]? Photo { get; set; } // Dodane pole na zdjęcie jako byte[]
        public IdentityUser? Author { get; set; } // Opcjonalnie: obiekt użytkownika jako autor
        public string AuthorID { get; set; } // Identyfikator autora quizu


    }
}
