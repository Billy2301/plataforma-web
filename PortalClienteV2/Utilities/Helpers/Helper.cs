using System.Globalization;

namespace PortalClienteV2.Utilities.Helpers
{
    public static class Helper
    {
        public static string GenerateShortGuid()
        {
            Guid guid = Guid.NewGuid();
            string guidString = guid.ToString("N");
            string shortGuid = guidString.Substring(0, 6);
            return shortGuid;
        }
        public static bool IsNumeric(string stringToTest)
        {
            double newVal;
            return double.TryParse(stringToTest, NumberStyles.Any,
            NumberFormatInfo.InvariantInfo, out newVal);
        }

        public static string ConvertirHora(float? horaDecimal)
        { 
            if(horaDecimal == null)
            {
                return "";
            }
            // Obtener la parte entera (horas) y la parte decimal
            int horas = (int)horaDecimal!;
            float? minutosDecimales = horaDecimal - horas;

            // Convertir la parte decimal a minutos
            int minutos = (int)(minutosDecimales * 60);

            // Formatear la hora y minutos en 'HH:mm'
            return $"{horas:D2}:{minutos:D2}";
        }
    }
}
