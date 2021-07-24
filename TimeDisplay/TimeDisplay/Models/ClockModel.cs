using System;
using System.Collections.Generic;
using System.Text;

namespace TimeDisplay.Models
{
    public class ClockModel
    {
        public string Name { get; set; }
        public TimeSpan TimeZoneDifferenceToUTC { get; set; }
        
    }
}
