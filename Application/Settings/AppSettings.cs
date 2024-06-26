using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Settings
{
    public class AppSettings
    {
        // Example setting for apps (map from appsetting.json)
        public string Secret { get; set; } = null!;
        public string Bucket { get; set; } = null!;
        public string DefaultFolder { get; set; } = null!;
        public string VNPayUrl { get; set; } = null!;
        public string ReturnUrl { get; set; } = null!;
        public string MerchantId { get; set; } = null!;
        public string MerchantPassword { get; set; } = null!;
    }
}
