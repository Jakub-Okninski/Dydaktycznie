namespace Dydaktycznie.Models
{
    public class QuestionAnswer
    {
        public int QuestionAnswerID { get; set; }
        public int QuizQuestionID { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }


    }
}
