using System;
using System.Collections.Generic;
using System.Text;

namespace TimeDisplay.Models
{
    public class ClockModel
    {
        private int id = -1;
        public int ID { get => id; set => id = value; }
        public string Name { get; set; }
        public TimeSpan TimeZoneDifferenceToUTC { get; set; }
        
    }
}
