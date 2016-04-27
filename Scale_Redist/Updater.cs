using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TweakScale
{
    public interface IRescalable
    {
        void OnRescale(ScalingFactor factor);
    }
    public interface IRescalable<T> : IRescalable
    {
    }
}