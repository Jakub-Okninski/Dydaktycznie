﻿namespace Dydaktycznie.Models
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<QuizQuestion> QuizQuestions { get; set; }

    }
}
