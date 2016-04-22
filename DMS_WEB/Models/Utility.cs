using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace DMS_WEB.Models {

    public enum UserTypes {
        Super_Admin = 100,
        Data_Entry = 200,
        Report_Viewer = 300, 
        Client_User = 400,
    }

    public enum TransmittalTypes { 
        IN = 1, 
        OUT = 2,
    }

    public class Utility {

        public static String GetLanIPAddress() {
            //The X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a 
            //client connecting to a web server through an HTTP proxy or load balancer
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip)) {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }

        public static string GetSessionID() {
            return HttpContext.Current.Session.SessionID;
        }

        public static string GetServerIPAddress() {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            return ipAddress.ToString();
        }

        public static string GetUserAgent() {
            return HttpContext.Current.Request.UserAgent;
        }

        public static string GetTime(DateTime date_val) {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (date_val - Jan1st1970).TotalMilliseconds.ToString();
        }

        public static string GetDateString(DateTime? date_val, string format) {
            string ret_val = string.Empty;
            if (date_val.HasValue) {
                ret_val = date_val.Value.ToString(format);
            }
            return ret_val;
        }
    } 
}