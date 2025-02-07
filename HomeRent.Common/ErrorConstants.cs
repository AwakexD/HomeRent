namespace HomeRent.Common
{
    public static class ErrorConstants
    {
        // General Booking errors
        public static string CreateBookingError = "Възникна грешка при създаването на резервацията";
        public static string CancelBookingError = "Резервацията не е намерена или не може да бъде отказана";
        public static string PropertyRetrievePriceError = "Грешка при взимането на цената на обявата";
        public static string GetBookedDatesError = "Възникна грешка при вземането на резервираните дати";
        public static string NotFoundUserError = "Няма намерен потребител";

        // Payment/BookingOverview specific errors
        public static string BookingAlreadyConfirmedError = "Резервацията вече е потвърдена.";
        public static string BookingNotFoundError = "Резервацията не е намерена.";
        public static string BookingOverviewRetrieveError = "Възникна грешка при извличането на обобщението на резервацията.";

        // Payment processing errors
        public static string InvalidPaymentMethodError = "Невалиден метод на плащане.";
        public static string PaymentProcessError = "Възникна грешка при обработката на плащането.";
        public static string PaymentConfirmationError = "Възникна грешка при потвърждаването на плащането.";
        public static string PaymentFailurePageError = "Възникна грешка при зареждането на страницата за неуспешно плащане.";

        // User error
        public static string UserNotFoundError = "Няма намерен потребител";

        // Property errors
        public static string PropertyNotFoundError = "Обявата не беше намерена";

    }
}
