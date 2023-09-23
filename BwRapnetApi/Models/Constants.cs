using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwRapnetApi.Models
{
    public class Constants
    {
        public static string SoapUrl = "https://technet.rapaport.com/webservices/Upload/DiamondManager.asmx";
        public static string LoginAction = "http://technet.rapaport.com/Login";
        public static string CheckIfLastUploadDone = "http://technet.rapaport.com/CheckIfLastUploadCompletedSuccessfully";
        public static string UploadStock = "http://technet.rapaport.com/UploadLots";
        public static string GetRapStock = "/RapApi/GetRapStock";

        public static string GetUploadStatus = " http://technet.rapaport.com/GetUploadStatus";
        //public static string LoginSoapXml = ""
    }
}