using Infra;

namespace GenerateIdentityServices.Services
{
    public static class GenerateIdentityExtenstionServices
    {
        public static string GenerateIdentityNumber(this long counter, int orderRequestType, int branchId, string branchNumber)
            => Encrypt(counter + branchId + branchNumber + orderRequestType.ToString("D2"));

        private static string Encrypt(string identity)
        {
            const int code = 3;
            long repo = 0;

            foreach (char item in identity)
            {
                repo += Convert.ToInt16(item) * code;
            }

            string digit = repo.ToString().Substring(repo.ToString().Length - 1, 1);

            return identity + digit;
        }

        public static string GenerateSerialFrom(this BaseAccountType orderRequestType,
            long lastSerial)
             => orderRequestType switch
             {
                 BaseAccountType.Individual => lastSerial.FromSerial().ToString("D7"),
                 BaseAccountType.Companies => lastSerial.FromSerial().ToString("D7"),
                 BaseAccountType.Certified => "",
                 _ => ""
             };

        private static long FromSerial(this long lastSerial)
         => lastSerial + 1;

        public static long EndSerial(this BaseAccountType orderRequestType, int chQuentity, long lastSerial)
            => orderRequestType switch
            {
                BaseAccountType.Individual => (chQuentity * 25) + lastSerial,
                BaseAccountType.Companies => (chQuentity * 50) + lastSerial,
                BaseAccountType.Certified => (chQuentity * 50) + lastSerial,
                _ => 0
            };
    }

}
