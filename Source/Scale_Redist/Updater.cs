using System.Collections.Generic;
using KSP;

namespace TweakScale
{
    public interface IRescalable
    {
        void OnRescale(ScalingFactor factor);
    }
    public interface IRescalable<T> : IRescalable{}

    public interface IFactory20
	{
        int getPriority();
        bool isSupported(Part part);
        HashSet<string> getSupportedModules();
        HashSet<string> getUnsupportedModules();
        
        IRescalable20 createRescalableFor(Part part);
        IDryCost20 createDryCostFor(Part part);
	}
    public interface IFactory20<T> : IFactory20{}

    public interface IRescalable20 : IRescalable
    {
        void OnUpdate();
        void OnShipModified();
    }
    public interface IRescalable20<T> : IRescalable20 {}

    public interface IDryCost20
	{
        float calculate();
	}
    public interface IDryCost20<T> : IDryCost20 {}
}