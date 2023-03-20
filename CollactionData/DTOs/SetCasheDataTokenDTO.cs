using System;
using System.Collections.Generic;
using System.Text;

namespace CollactionData.DTOs
{
    public class SetCacheDataTokenDTO
    {
        public string UserID { get; set; }
        public string Token { get; set; }
        public string IdRefreshToken { get; set; }
    }
}
