namespace LMS.api.Extensions
{
    public static class DateTimeExtension
    {
        private static Random random = new Random();
        public static DateTime MoveToFuture(this DateTime value) {

            int days = random.Next(0, 100);
            int hours = random.Next(0, 100);

            return DateTime.Now.AddDays(days);
                
        }
    }
}
