﻿namespace Dydaktycznie.Models
{
    public class QuizQuestion
    {
        public int QuizQuestionID { get; set; }
        public int QuizID { get; set; }
        public string Question { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }

    }
}