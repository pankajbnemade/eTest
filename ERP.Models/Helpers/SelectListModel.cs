using Newtonsoft.Json;
using System.Collections.Generic;

namespace ERP.Models.Helpers
{
    /// <summary>
    /// class for select list model.
    /// </summary>
    public class SelectListModel
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class MultiSelectGroupModel
    {
        [JsonIgnore]
        public string label { get; set; }
        public List<MultiSelectGroupOptionModel> children { get; set; }
    }

    public class MultiSelectGroupOptionModel
    {
        public string label { get; set; }
        public string value { get; set; }
        public string selected { get; set; }
    }
}
