using System;
using System.Collections.Generic;

namespace IdentityDemo.Models.Entities
{
    public partial class User
    {
        public string Id { get; set; } = null!;

        public virtual AspNetUser IdNavigation { get; set; } = null!;
    }
}
