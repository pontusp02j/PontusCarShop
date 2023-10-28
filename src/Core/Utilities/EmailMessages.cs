namespace Core.Utilities
{
    public static class EmailMessages
    {
        public static string VerificationEmail(string username, string link)
        {
            return $"Dear {username},<br /><br />" +
                    "Please verify your email address by clicking the link below:<br /><br />" +
                    $"<a href=\"{link}\">{link}</a><br /><br />" +
                    "Regards,<br />" +
                    "The Car Shop";
        }

        public static string NewCarsNotificationEmail(string username, IEnumerable<string> newCars, string date)
        {
            return $"Dear {username},<br /><br />" +
                $"Here is a list of all the new cars for sale that have been added since {date}:<br /><br />" +
                string.Join("<br />", newCars) +
                "<br />Regards,<br />" +
                "The Car Shop";
        }

        public static string NewCarsImmidiateNotificationEmail(string username, string modelName)
        {
            return $"Dear {username},<br /><br />" +
                    $"A new {modelName} has been added for sale!<br /><br />" +
                    "Regards,<br />" +
                    "The Car Shop";
        }
    }
}
