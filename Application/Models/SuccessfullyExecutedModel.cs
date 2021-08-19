namespace Application.Models
{
    public class SuccessfullyExecutedModel
    {
        public SuccessfullyExecutedModel(bool executedSuccessfully)
        {
            ExecutedSuccessfully = executedSuccessfully;
        }

        public bool ExecutedSuccessfully { get; }
    }
}