using System;
using System.Collections.Generic;
using System.Text;

namespace PGNLibrary
{
    public class TimeControl
    {
        int TimeLimit { get; set; }
        int Increment { get; set; }

        public TimeControl(int timeLimit, int increment)
        {
            TimeLimit = timeLimit;
            Increment = increment;
        }
    }
}
