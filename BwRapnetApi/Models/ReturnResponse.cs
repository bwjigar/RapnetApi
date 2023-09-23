using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwRapnetApi.Models
{
    public class ReturnResponse
    {
    }
    public class LastUploadResponse
    {
        public string PercentFinished { get; set; }
        public string uploadStatus { get; set; }
    //    public string Timestamp { get; set; }
        public string UploadID { get; set; }
    //    public string CurrentStepName { get; set; }
        public string RowsValid { get; set; }
        public string RowsInvalid { get; set; }
        public string RowsReceived { get; set; } 
    
    }
}