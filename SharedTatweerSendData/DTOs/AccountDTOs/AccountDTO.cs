using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTatweerSendData.DTOs.AccountDTOs
{
    public class AccountDTO
    {
        public string id { get; set; }
        public string accountName { get; set; }
        public string accountNo { get; set; }
        public string bankId { get; set; }
        public string bankName{ get; set; }
        public string regionName { get; set; }
        public string regionId { get; set; }
        public string bankRegionId { get; set; }

        public string branchName { get; set; }
        public string branchId { get; set; }
        public string branchNo { get; set; }
        public string phoneNumber { get; set; }
        //public string accountType { get; set; }
        public int accountTypeValue { get; set; }
        //public bool accountState { get; set; }
        public int accountStateValue { get; set; }
        public bool printExternally { get; set; }

    }
}
