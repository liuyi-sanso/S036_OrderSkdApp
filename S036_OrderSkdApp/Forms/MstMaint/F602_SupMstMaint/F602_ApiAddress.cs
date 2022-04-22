using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S036_OrderSkdApp
{
    /// <summary>
    /// 住所情報Results
    /// </summary>
    public class F602_ApiAddress
    {
        public string message { get; set; }
        public List<F602_Address> results { get; set; }
        public int status { get; set; }
    }
}
