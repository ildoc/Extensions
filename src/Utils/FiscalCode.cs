using System;
using System.Linq;

namespace Utils
{
    public static class FiscalCode
    {
        private const string vowels = "aeiou";
        private static string consonants => "abcdefghijklmnopqrstuvwxz".Except(vowels);

        private static string Except(this string text1, string text2)
        {
            return string.Join("", text1.ToCharArray().Where(x => !text2.Contains(x)));
        }

        public static bool EasyCheck(string fiscalCode, string name, string surname, DateTime birthdate)
        {

            var fcSurname = fiscalCode[0..3];
            var fcName = fiscalCode[3..6];
            var fcBirthYear = fiscalCode[6..8];
            var fcBirthMonth = fiscalCode[8];
            var fcBirthDay = fiscalCode[9..11];

            var checkedName = name.Except(vowels) == fcName;
            //var checkedSurname = surname.Except

            return false;
        }

        public static bool CheckSurname(string fcSurname, string surname)
        {
            var check = "";
            var surnameConsonants = surname.Except(vowels);
            if (surnameConsonants.Length < 3)
            {
                check = surnameConsonants + surname.Substring(surname.IndexOf(surnameConsonants.TakeLast(1).First()) + 1, 3 - surnameConsonants.Length);
            }
            else
            {
                check = surnameConsonants[0..3];
            }
            return check == fcSurname;
        }

        public static bool CheckName(string fcSurname, string surname)
        {
            var check = "";
            var surnameConsonants = surname.Except(vowels);
            if (surnameConsonants.Length < 3)
            {
                check = surnameConsonants + surname.Substring(surname.IndexOf(surnameConsonants.TakeLast(1).First()) + 1, 3 - surnameConsonants.Length);
            }
            else
            {
                check = surnameConsonants[0..3];
            }
            return check == fcSurname;
        }
    }
}
