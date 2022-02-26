using System.ComponentModel;

namespace ERP.Models.Utility
{
    public enum StorageType
    {
        [Description("File")]
        File = 1,

        [Description("Azure")]
        Azure = 2,
    }
   
}
