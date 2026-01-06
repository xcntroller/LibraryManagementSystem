namespace LibraryManagementSystem.Validators
{
    public class IsbnValidator
    {
        // Validates an ISBN-10 or ISBN-13 code
        public static bool IsValidISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            isbn = isbn.Replace("-", "").Replace(" ", "");

            // Check if it's ISBN-10 or ISBN-13
            if (isbn.Length == 10)
                return IsValidISBN10(isbn);
            else if (isbn.Length == 13)
                return IsValidISBN13(isbn);

            return false;
        }

        // Validates ISBN-10
        private static bool IsValidISBN10(string isbn)
        {
            if (!isbn.All(c => char.IsDigit(c) || c == 'X' || c == 'x'))
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
                sum += (isbn[i] - '0') * (10 - i);
            }

            char lastChar = isbn[9];
            int checkDigit = lastChar == 'X' || lastChar == 'x' ? 10 : lastChar - '0';
            sum += checkDigit;

            return sum % 11 == 0;
        }

        // Validates ISBN-13
        private static bool IsValidISBN13(string isbn)
        {
            if (!isbn.All(char.IsDigit))
                return false;

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = isbn[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = isbn[12] - '0';
            int calculatedCheckDigit = (10 - (sum % 10)) % 10;

            return checkDigit == calculatedCheckDigit;
        }
    }
}
