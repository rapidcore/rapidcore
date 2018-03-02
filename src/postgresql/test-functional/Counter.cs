using System;

namespace RapidCore.PostgreSql.FunctionalTests
{
    public class Counter
    {
        public int Id { get; set; }
        public long CounterValue { get; set; }
        public DateTime At { get; set; }
        public string Description { get; set; }
    }
}
