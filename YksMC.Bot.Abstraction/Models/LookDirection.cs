using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Models
{
    public class LookDirection
    {
        public double Pitch { get; }
        public double Yaw { get; }

        public LookDirection(double pitch, double yaw)
        {
            Pitch = pitch;
            Yaw = yaw;
        }
    }
}
