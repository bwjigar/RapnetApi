using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using BwRapnetApi.Models;
using Oracle.DataAccess.Client;

namespace BwRapnetApi.Controllers
{
    public class APIController : Controller
    {
        // GET: API
        public ActionResult Index()
        {
         //   var str = TestSP();
          //var Tiket = GetTiket();
            return View();
        }

        public JsonResult CheckIfLastUploadDone()
        {
            LastUploadResponse Result = new LastUploadResponse();
            var res = "";
            try
            {
                var Tiket = GetTiket();
                var _url = Constants.SoapUrl;
                var _action = Constants.CheckIfLastUploadDone;
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);

                XmlDocument soapEnvelopeXml = CreateSoapForCheckLastUploadDone(Tiket);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(soapResult);
                        XmlElement root = xd.DocumentElement;
                        Result.uploadStatus = root.GetElementsByTagName("uploadStatus")[0].InnerText.ToString();
                        Result.PercentFinished = root.GetElementsByTagName("PercentFinished")[0].InnerText.ToString();
                        //Result.Timestamp = root.GetElementsByTagName("Timestamp")[0].InnerText.ToString();
                        //Result.CurrentStepName = root.GetElementsByTagName("CurrentStepName")[0].InnerText.ToString();
                        Result.RowsValid = root.GetElementsByTagName("RowsValid")[0].InnerText.ToString();
                        Result.RowsInvalid = root.GetElementsByTagName("RowsInvalid")[0].InnerText.ToString();
                        Result.RowsReceived = root.GetElementsByTagName("RowsReceived")[0].InnerText.ToString();
                     

                    }
                }
                return Json(new { Result }, JsonRequestBehavior.AllowGet);
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return Json(new { res }, JsonRequestBehavior.AllowGet);
        }


        public string GetTiket()
        {
            try
            {
                string Tiket = "";
                var _url = "https://technet.rapaport.com/webservices/Upload/DiamondManager.asmx";
                var _action = "http://technet.rapaport.com/Login";
                XmlDocument soapEnvelopeXml = CreateSoapForLogin();
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }

                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(soapResult);
                    XmlElement root = xd.DocumentElement;
                    XmlNodeList titleList = root.GetElementsByTagName("Ticket");
                    Tiket = titleList[0].InnerText;
                }
                return Tiket;
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return "";
        }


        public JsonResult CallWebServiceComman()
        {
            try
            {
                var _url = Constants.SoapUrl;
                var _action = Constants.LoginAction;

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                // get the response from the completed web request.
                string soapResult; string Tiket = "";
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }

                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(soapResult);
                    XmlElement root = xd.DocumentElement;
                    XmlNodeList titleList = root.GetElementsByTagName("Ticket");
                    Tiket = titleList[0].InnerText;
                }
                return Json(new { Tiket }, JsonRequestBehavior.AllowGet);
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return Json("OK");
        }



        private static XmlDocument CreateSoapForLogin()
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(
          @"<soap:Envelope  xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
         <soap:Body>
             <Login xmlns=""http://technet.rapaport.com/"">
                <Username>pateljignesh</Username>
                <Password>choco1402</Password>
            </Login>
         </soap:Body>
</soap:Envelope>");


            return soapEnvelopeDocument;
        }

        public JsonResult CallWebService()
        {
            try
            {
                var _url = Constants.SoapUrl;
                var _action = Constants.LoginAction;

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                // get the response from the completed web request.
                string soapResult; string Tiket = "";
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }

                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(soapResult);
                    XmlElement root = xd.DocumentElement;
                    XmlNodeList titleList = root.GetElementsByTagName("Ticket");
                    Tiket = titleList[0].InnerText;
                }
                return Json(new { Tiket }, JsonRequestBehavior.AllowGet);
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return Json("OK");
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.SendChunked = true;
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(
            @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
        <soap:Body>
             <Login xmlns=""http://technet.rapaport.com/"">
                <Username>vrushali139</Username>
                <Password>Shairu9012</Password>
            </Login>
         </soap:Body>
</soap:Envelope>");
            return soapEnvelopeDocument;
        }

        private static XmlDocument CreateSoapForCheckLastUploadDone(string Tiket)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(
            @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
         <soap:Header>
           <AuthenticationTicketHeader xmlns=""http://technet.rapaport.com/"">
             <Ticket>" + Tiket.ToString() + @"</Ticket>
           </AuthenticationTicketHeader>
         </soap:Header>
         <soap:Body>
         <CheckIfLastUploadCompletedSuccessfully xmlns = ""http://technet.rapaport.com/"" />
         
         </soap:Body>
</soap:Envelope>");
            return soapEnvelopeDocument;   
          
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }


        public JsonResult UploadStock()
        {
            var res = "";
            try
            {

                string StrStock = "";
                
                StrStock = GetRapNetStockString();
                    if (StrStock.ToString() != "")
                {
                    var Tiket = GetTiket();
                    var _url = Constants.SoapUrl;//"https://technet.rapaport.com/webservices/Upload/DiamondManager.asmx";
                    var _action = Constants.UploadStock; //"http://technet.rapaport.com/UploadLots";
                    HttpWebRequest webRequest = CreateWebRequest(_url, _action);

                    XmlDocument soapEnvelopeXml = CreateSoapForUploadStock(Tiket, StrStock);
                    InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                    IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                    asyncResult.AsyncWaitHandle.WaitOne();
                    string soapResult;
                    using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            soapResult = rd.ReadToEnd();
                            XmlDocument xd = new XmlDocument();
                            xd.LoadXml(soapResult);
                            XmlElement root = xd.DocumentElement;



                        }
                    }
                    return Json(new { soapResult }, JsonRequestBehavior.AllowGet);
                }


                //return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return Json(new { res }, JsonRequestBehavior.AllowGet);
            
        }


        public string GetRapNetStockString()
        {

            Oracle_DBAccess oracleDbAccess = new Oracle_DBAccess();
            List<OracleParameter> paramList = new List<OracleParameter>();
          
            try
            {
                //  req = JsonConvert.DeserializeObject<TransValueRequest>(data.ToString());

                OracleParameter param1 = new OracleParameter("p_for_comp", OracleDbType.Int32);
                param1.Value = 1;
                paramList.Add(param1);

                OracleParameter param2 = new OracleParameter("p_for_type", OracleDbType.Varchar2);
                param2.Value = "R";
                paramList.Add(param2);

                OracleParameter param3 = new OracleParameter("p_for_Date", OracleDbType.Date);
                param3.Value = DateTime.Now;
                paramList.Add(param3);

                OracleParameter param4 = new OracleParameter("vrec", OracleDbType.RefCursor);
                param4.Direction = ParameterDirection.Output;
                paramList.Add(param4);

                System.Data.DataTable dt = oracleDbAccess.CallSP("get_live_data", paramList);
                var str = "";
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        str = GetDtToCsv(dt);
                        //  UploadStock(str);
                    }
                }
                return str;
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                oracleDbAccess = null;
                paramList = null;
              
            }
            return "";
        }

        public string TestSP()
        {

            Oracle_DBAccess oracleDbAccess = new Oracle_DBAccess();
            List<OracleParameter> paramList = new List<OracleParameter>();

            try
            {
                //  req = JsonConvert.DeserializeObject<TransValueRequest>(data.ToString());

                OracleParameter param1 = new OracleParameter("p_for_comp", OracleDbType.Int32);
                param1.Value = 1;
                paramList.Add(param1);

             

                OracleParameter param4 = new OracleParameter("vrec", OracleDbType.RefCursor);
                param4.Direction = ParameterDirection.Output;
                paramList.Add(param4);

                System.Data.DataTable dt = oracleDbAccess.CallSP("UPDATE_SINWARDDET", paramList);
                var str = "";
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        //str = GetDtToCsv(dt);
                        //  UploadStock(str);
                    }
                }
                return str;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oracleDbAccess = null;
                paramList = null;

            }
            return "";
        }
        public string GetDtToCsv(DataTable dt)
        {

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => "\"" + field.ToString() + "\"");
                sb.AppendLine(string.Join(",", fields));
            }
            return sb.ToString();

        }

        private static XmlDocument CreateSoapForUploadStock(string Tiket, string stock)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            try
            {
             //   XmlDocument soapEnvelopeDocument = new XmlDocument();
                soapEnvelopeDocument.LoadXml(
                 @"<soap:Envelope  xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
         <soap:Header>
           <AuthenticationTicketHeader xmlns=""http://technet.rapaport.com/"">
             <Ticket>" + Tiket.ToString() + @"</Ticket>
           </AuthenticationTicketHeader>
         </soap:Header>
         <soap:Body>
 <UploadLots xmlns=""http://technet.rapaport.com/"">
      <Parameters>
        <LotList> " + stock.Replace("'", "&apos;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;") + @" </LotList>
        <LotListFormat>Rapnet</LotListFormat>
        <ReplaceAll>true</ReplaceAll>
        <FirstRowHeaders>true</FirstRowHeaders>
        <ReportOption>None</ReportOption>
      </Parameters>
    </UploadLots>
           
         </soap:Body>
</soap:Envelope>");
                return soapEnvelopeDocument;
            }
            catch (Exception ex) {
                Console.Write(stock);
            }
            return soapEnvelopeDocument;
        }
    }
}