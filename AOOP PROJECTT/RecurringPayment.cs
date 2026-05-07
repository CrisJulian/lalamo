using System;
using System.Collections.Generic;
using System.Text;

namespace AOOP_PROJECTT
{
    public class RecurringPayment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Frequency { get; set; }
        public DateTime NextDate { get; set; }
        public string Category { get; set; }
    }
}
