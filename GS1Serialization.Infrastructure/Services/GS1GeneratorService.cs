using GS1Serialization.Application.Interfaces;
using System.Text;

namespace GS1Serialization.Infrastructure.Services
{
    public class GS1GeneratorService : IGS1GeneratorService
    {
        private const string AI_GTIN = "01";
        private const string AI_EXP_DATE = "17";
        private const string AI_BATCH_LOT = "10";
        private const string AI_SERIAL = "21";
        private const string AI_SSCC = "00";

        public string GenerateGS1String(string gtin, string lot, DateOnly expireDate, string serialNumber)
        {
            var sb = new StringBuilder();

            // 1. GTIN (AI:01) - Sabit Uzunluk (14 hane)
            sb.Append($"({AI_GTIN}){gtin}");

            // 2. Son Kullanma Tarihi (AI:17) - Format: YYMMDD
            var formattedDate = expireDate.ToString("yyMMdd");
            sb.Append($"({AI_EXP_DATE}){formattedDate}");

            // 3. Batch/Lot (AI:10) - Değişken Uzunluk
            sb.Append($"({AI_BATCH_LOT}){lot}");

            // 4. Seri Numarası (AI:21) - Değişken Uzunluk
            sb.Append($"({AI_SERIAL}){serialNumber}");

            return sb.ToString();
        }
        public string GenerateSSCC(string extensionDigit, string companyPrefix, string serialReference)
        {
            // SSCC Algoritması: (Extension) + (Company Prefix) + (Serial Ref) + (Check Digit) -> Toplam 18 hane
            var rawData = $"{extensionDigit}{companyPrefix}{serialReference}";
            var checkDigit = CalculateCheckDigit(rawData);
            return $"({AI_SSCC}){rawData}{checkDigit}";
        }
        private int CalculateCheckDigit(string data)
        {
            var sum = 0;
            var odd = true; // Sağdan sola giderken tek/çift pozisyon

            for (var i = data.Length - 1; i >= 0; i--)
            {
                var digit = int.Parse(data[i].ToString());
                sum += odd ? digit * 3 : digit;
                odd = !odd;
            }

            var remainder = sum % 10;
            return remainder == 0 ? 0 : 10 - remainder;
        }
    }
}
