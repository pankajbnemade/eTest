using System.Collections.Generic;

namespace ERP.Models.Helpers
{
    public class JsonData<T>
    {
        public JsonData()
        {
            Result = default(T);
            EventMessages = new List<EventData>();
        }

        public JsonData(T result)
        {
            Result = result;
            EventMessages = new List<EventData>();
        }

        public List<EventData> EventMessages { get; set; }
        public T Result { get; set; }
    }

    public class EventData
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Class { get; set; }

        public EventData()
        {
            this.Key = default(string);
            this.Value = default(string);
            this.Class = default(string);
        }

        public EventData(string key, string value, string className)
        {
            this.Key = key;
            this.Value = value;
            this.Class = className;
        }
    }
   
    public class JsonStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
