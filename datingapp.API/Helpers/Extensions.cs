using System;

namespace datingapp.API.Helpers
{
    public static class Extensions
    {
        public static int CalculateAge(this DateTime thisDateTime) {
            var age = DateTime.Today.Year - thisDateTime.Year;

            if(thisDateTime.AddYears(age) > DateTime.Today) {
                age--;
            }

            return age;
        }
    }
}