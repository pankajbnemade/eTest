using ERP.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UAParser;

namespace ERP.Models.Logger
{
    public class SeriLogTools
    {
        public SeriLogTools() { }

        /// <summary>
        /// Logs the warning with request parameters.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="httpContext"></param>
        public static void LogWarningWithContext(Exception exception)
        {
            try
            {
                LogModel logModel = new LogModel();

                GetExceptionData(LogEventLevel.Warning, exception, ref logModel);

                if (ContextAccessor.HttpContext != null)
                {
                    GetRequestData(ContextAccessor.HttpContext, ref logModel);
                    GetCookiesData(ContextAccessor.HttpContext, ref logModel);
                    GetSessionData(ContextAccessor.HttpContext, ref logModel);
                }

                PrepareLogContext(logModel);

                Log.Logger.Warning(exception, exception.InnerException != null ? exception.InnerException.Message : exception.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to log the warning, please debug the custom warning logging process");
            }
        }

        /// <summary>
        /// Logs the error with request parameters.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="httpContext"></param>
        public static void LogErrorWithContext(Exception exception)
        {
            try
            {
                LogModel logModel = new LogModel();

                GetExceptionData(LogEventLevel.Error, exception, ref logModel);



                if (ContextAccessor.HttpContext != null)
                {
                    GetRequestData(ContextAccessor.HttpContext, ref logModel);
                    GetCookiesData(ContextAccessor.HttpContext, ref logModel);
                    GetSessionData(ContextAccessor.HttpContext, ref logModel);

                    //SendErrorMail(ContextAccessor.HttpContext, logModel);
                }

                PrepareLogContext(logModel);

                Log.Logger.Error(exception, exception.InnerException != null ? exception.InnerException.Message : exception.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to log the error, please debug the custom error logging process");
            }
        }

        /// <summary>
        /// Logs the fatal with request parameters.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="httpContext"></param>
        public static void LogFatalWithContext(Exception exception)
        {
            try
            {
                LogModel logModel = new LogModel();

                GetExceptionData(LogEventLevel.Fatal, exception, ref logModel);

                if (ContextAccessor.HttpContext != null)
                {
                    GetRequestData(ContextAccessor.HttpContext, ref logModel);
                    GetCookiesData(ContextAccessor.HttpContext, ref logModel);
                    GetSessionData(ContextAccessor.HttpContext, ref logModel);

                    //SendErrorMail(ContextAccessor.HttpContext, logModel);
                }

                PrepareLogContext(logModel);

                Log.Logger.Fatal(exception, exception.InnerException != null ? exception.InnerException.Message : exception.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to log the fatal, please debug the custom fatal logging process");
            }
        }

        private static void PrepareLogContext(LogModel logModel)
        {
            LogContext.PushProperty("Guid", logModel.Guid);
            if (!string.IsNullOrEmpty(logModel.UserName)) LogContext.PushProperty("UserName", logModel.UserName);
            if (!string.IsNullOrEmpty(logModel.OS)) LogContext.PushProperty("OS", logModel.OS);
            if (!string.IsNullOrEmpty(logModel.Device)) LogContext.PushProperty("Device", logModel.Device);
            if (!string.IsNullOrEmpty(logModel.Browser)) LogContext.PushProperty("Browser", logModel.Browser);
            if (!string.IsNullOrEmpty(logModel.IsSecureConnection.ToString())) LogContext.PushProperty("IsSecureConnection", logModel.IsSecureConnection);
            if (!string.IsNullOrEmpty(logModel.LocalIpaddress)) LogContext.PushProperty("LocalIPAddress", logModel.LocalIpaddress);
            if (!string.IsNullOrEmpty(logModel.RemoteIpaddress)) LogContext.PushProperty("RemoteIPAddress", logModel.RemoteIpaddress);
            if (!string.IsNullOrEmpty(logModel.UrlReferrer)) LogContext.PushProperty("UrlReferrer", logModel.UrlReferrer);
            if (!string.IsNullOrEmpty(logModel.RawUrl)) LogContext.PushProperty("RawUrl", logModel.RawUrl);
            if (!string.IsNullOrEmpty(logModel.Area)) LogContext.PushProperty("Area", logModel.Area);
            if (!string.IsNullOrEmpty(logModel.Controller)) LogContext.PushProperty("Controller", logModel.Controller);
            if (!string.IsNullOrEmpty(logModel.Action)) LogContext.PushProperty("Action", logModel.Action);
            if (!string.IsNullOrEmpty(logModel.HttpMethod)) LogContext.PushProperty("HttpMethod", logModel.HttpMethod);
            if (!string.IsNullOrEmpty(logModel.QueryString)) LogContext.PushProperty("QueryString", logModel.QueryString);
            if (!string.IsNullOrEmpty(logModel.FormData)) LogContext.PushProperty("FormData", logModel.FormData);
            if (!string.IsNullOrEmpty(logModel.CookieData)) LogContext.PushProperty("CookieData", logModel.CookieData);
            if (!string.IsNullOrEmpty(logModel.SessionData)) LogContext.PushProperty("SessionData", logModel.SessionData);
        }

        private static void GetExceptionData(LogEventLevel logEventLevel, Exception exception, ref LogModel logModel)
        {
            logModel.Guid = Guid.NewGuid();
            logModel.TimeStamp = DateTime.Now;
            logModel.Level = logEventLevel.ToString();
            logModel.Type = exception.GetType().FullName;
            logModel.Message = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
            logModel.Exception = exception.ToString();
        }

        private static void GetCookiesData(HttpContext httpContext, ref LogModel logModel)
        {
            try
            {
                if (httpContext.Request != null && httpContext.Request.Cookies != null)
                    logModel.CookieData = JsonConvert.SerializeObject(httpContext.Request.Cookies.ToDictionary(x => x.Key, y => y.Value.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetSessionData(HttpContext httpContext, ref LogModel logModel)
        {
            try
            {
                if (httpContext.Session != null)
                {
                    if (!string.IsNullOrEmpty(httpContext.Session.GetString("UserName")))
                        logModel.UserName = httpContext.Session.GetString("UserName");

                    string[] ignoredSessionKeys = { "SeriLogActionArguments", "ActionEndCount", "ActionStartCount" };

                    List<KeyValuePair<string, string>> sessionData = new List<KeyValuePair<string, string>>();
                    foreach (var key in httpContext.Session.Keys.Where(x => !ignoredSessionKeys.Contains(x)))
                        sessionData.Add(new KeyValuePair<string, string>(key, httpContext.Session.GetString(key)));

                    if (sessionData.Any())
                    {
                        logModel.SessionData = JsonConvert.SerializeObject(sessionData.ToDictionary(x => x.Key, y => y.Value));
                        httpContext.Session.Remove("SeriLogActionArguments");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetRequestData(HttpContext httpContext, ref LogModel logModel)
        {
            try
            {
                var request = httpContext.Request;

                if (request != null)
                {
                    Parser uaParser = Parser.GetDefault();
                    ClientInfo clientInfo = uaParser.Parse(request.Headers["User-Agent"].ToString());
                    logModel.OS = clientInfo?.OS?.ToString();
                    logModel.Device = clientInfo?.Device?.ToString();
                    logModel.Browser = clientInfo?.UA?.ToString();

                    logModel.IsSecureConnection = request.IsHttps;
                    logModel.LocalIpaddress = request.HttpContext.Connection?.LocalIpAddress?.ToString();
                    logModel.RemoteIpaddress = request.HttpContext.Connection?.RemoteIpAddress?.ToString();
                    logModel.UrlReferrer = request.Headers["Referer"].ToString();
                    logModel.RawUrl = request.Path.Value;

                    var routeData = httpContext.GetRouteData();
                    if (routeData != null)
                    {
                        logModel.Area = routeData.Values["area"]?.ToString();
                        logModel.Controller = routeData.Values["controller"]?.ToString();
                        logModel.Action = routeData.Values["action"]?.ToString();
                    }

                    logModel.HttpMethod = request.Method;

                    if (request.QueryString.HasValue)
                        logModel.QueryString = request.QueryString.Value;

                    if (httpContext.Session.Keys.Contains("SeriLogActionArguments"))
                        logModel.FormData = httpContext.Session.GetString("SeriLogActionArguments");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
