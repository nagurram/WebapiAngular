using System;

namespace LSDataApi.DBContext
{
    public partial class UserToken
    {
        public int UserTokenId { get; set; }
        public int ResourceId { get; set; }
        public DateTime TokenValidFrom { get; set; }
        public DateTime TokenValidUntil { get; set; }
        public string PrivateKey { get; set; }
        public string AccessToken { get; set; }

        public virtual Resource Resource { get; set; }
    }
}