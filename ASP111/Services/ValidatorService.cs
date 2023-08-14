using System.Text.RegularExpressions;
namespace ASP111.Services
{
    public class ValidatorService
    {
        public bool ValidateNonEmpty(string input)
        {
            //  Проверяем, что входная строка не пустая и не содержит Пробельных символов
            return !string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input);
        }

        public bool ValidateLogin(string input)
        {
            // Проверяем, что входная строка не пустая и состоит только из символов, допустимых в именах переменных
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^[a-zA-Z_][a-zA-Z0-9_]*$");
        }
    }
}
