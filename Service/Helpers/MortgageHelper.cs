namespace Service.Helpers
{
    /// <summary>
    /// Class for calling helper functions
    /// </summary>
    public class MortgageHelper
    {
        public static int CalculateAge(DateOnly birthDate)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            int age = currentDate.Year - birthDate.Year;

            if (currentDate.DayOfYear < birthDate.DayOfYear)
                age--;

            return age;
        }
    }
}
