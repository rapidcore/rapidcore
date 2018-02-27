using System;
using System.Collections.Generic;
using System.Text;

namespace functionaltests.Migrations
{
    public class Counter
    {
        public int Id { get; set; }
        public long CounterValue { get; set; }
        public DateTime At { get; set; }
        public string Description { get; set; }
    }
}
