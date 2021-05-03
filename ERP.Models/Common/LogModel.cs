using System;

namespace ERP.Models.Common
{
    public class LogModel
    {
        public Guid Guid { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string OS { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public bool? IsSecureConnection { get; set; }
        public string LocalIpaddress { get; set; }
        public string RemoteIpaddress { get; set; }
        public string UrlReferrer { get; set; }
        public string RawUrl { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string HttpMethod { get; set; }
        public string QueryString { get; set; }
        public string FormData { get; set; }
        public string CookieData { get; set; }
        public string SessionData { get; set; }
    }
}
