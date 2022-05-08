using System;
using System.Collections.Generic;

namespace PlannerAPI.Entities
{
    public partial class ActionContact
    {
        public long ActionId { get; set; }
        public long ContactId { get; set; }
    }
}
