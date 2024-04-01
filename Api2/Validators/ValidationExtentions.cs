using System.Globalization;
using System.Text;

namespace Api2.Validators
{
    public static class ValidationExtentions
    {
        private static int GetFirstCharAsciiCode(string value)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
            return asciiBytes[0];
        }

        public static bool IsValidCui(this string cui)
        {
            if (cui.Length > 10 || cui.Length < 2) return false;

            var codnew = cui;
            var padd = 10 - cui.Length;

            if (padd > 0)
                for (var index = 0; index < padd; index++) { codnew = '0' + codnew; }

            double total = 0;
            total += (GetFirstCharAsciiCode(codnew.Substring(0, 1)) - 48) * 7;
            total += (GetFirstCharAsciiCode(codnew.Substring(1, 1)) - 48) * 5;
            total += (GetFirstCharAsciiCode(codnew.Substring(2, 1)) - 48) * 3;
            total += (GetFirstCharAsciiCode(codnew.Substring(3, 1)) - 48) * 2;
            total += (GetFirstCharAsciiCode(codnew.Substring(4, 1)) - 48) * 1;
            total += (GetFirstCharAsciiCode(codnew.Substring(5, 1)) - 48) * 7;
            total += (GetFirstCharAsciiCode(codnew.Substring(6, 1)) - 48) * 5;
            total += (GetFirstCharAsciiCode(codnew.Substring(7, 1)) - 48) * 3;
            total += (GetFirstCharAsciiCode(codnew.Substring(8, 1)) - 48) * 2;

            total *= 10;
            total -= Math.Floor(total / 11) * 11;

            if (total == 10) total = 0;
            if (total == GetFirstCharAsciiCode(codnew.Substring(9, 1)) - 48)
                return true;

            return false;
        }
    }
}
