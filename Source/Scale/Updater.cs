using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TweakScale
{
    public abstract class RescalableRegistratorAddon : MonoBehaviour
    {
        private static bool _loadedInScene;

        public void Start()
        {
            if (_loadedInScene)
            {
                Destroy(gameObject);
                return;
            }
            _loadedInScene = true;
            OnStart();
        }

        public abstract void OnStart();

        public void Update()
        {
            _loadedInScene = false;
            Destroy(gameObject);
        }
    }

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class TweakScaleRegister : RescalableRegistratorAddon
    {
        override public void OnStart()
        {
            foreach (Type gen in Tools.GetAllTypes()
                .Where(IsGenericFactory)
                .ToArray()
            ) {
                Type t = gen.GetInterfaces()
                    .First(a => a.IsGenericType &&
                    a.GetGenericTypeDefinition() == typeof(IFactory20<>));

                RegisterGenericFactory(gen, t.GetGenericArguments()[0]);
            }

            foreach (Type gen in Tools.GetAllTypes()
                .Where(IsGenericRescalable)
                .ToArray()
            ) {
				Type t = gen.GetInterfaces()
                    .First(a => a.IsGenericType &&
                    a.GetGenericTypeDefinition() == typeof(IRescalable<>));

                RegisterGenericRescalable(gen, t.GetGenericArguments()[0]);
            }
        }

        private static void RegisterGenericFactory(Type resc, Type arg)
        {
            ConstructorInfo c = resc.GetConstructor(new[] { arg });
            if (c == null)
                return;
            Func<PartModule, IFactory20> creator = pm => (IFactory20)c.Invoke(new object[] { pm });

            TweakScaleUpdater.RegisterUpdater(arg, creator);
        }

        private static bool IsGenericFactory(Type t)
        {
            return !t.IsGenericType && t.GetInterfaces()
                .Any(a => a.IsGenericType &&
                a.GetGenericTypeDefinition() == typeof(IFactory20<>));
        }

        private static void RegisterGenericRescalable(Type resc, Type arg)
        {
			ConstructorInfo c = resc.GetConstructor(new[] { arg });
            if (c == null)
                return;
            Func<PartModule, IRescalable> creator = pm => (IRescalable)c.Invoke(new object[] { pm });

            TweakScaleUpdater.RegisterUpdater(arg, creator);
        }

        private static bool IsGenericRescalable(Type t)
        {
            return !t.IsGenericType && t.GetInterfaces()
                .Any(a => a.IsGenericType &&
                a.GetGenericTypeDefinition() == typeof(IRescalable<>));
        }
        
    }

    static class TweakScaleUpdater
    {
        // Every kind of updater is registered here, and the correct kind of updater is created for each PartModule.
        // FIXME: Such updaters need to be enabled on a user configurable setting!
        internal static readonly Dictionary<Type, Func<PartModule, IRescalable>> Ctors = new Dictionary<Type, Func<PartModule, IRescalable>>();
        internal static readonly Dictionary<Type, Func<PartModule, IFactory20>> Factories = new Dictionary<Type, Func<PartModule, IFactory20>>();

        internal static void RegisterUpdater(Type pm, Func<PartModule, IFactory20> creator)
        {
            // FIXME: Isso não tá certo! Tem que checar se o partmodule é compatível no IFactory!
            Factories[pm] = creator;
        }

        /// <summary>
        /// Registers an updater for partmodules of type <paramref name="pm"/>.
        /// </summary>
        /// <param name="pm">Type of the PartModule type to update.</param>
        /// <param name="creator">A function that creates an updater for this PartModule type.</param>
        public static void RegisterUpdater(Type pm, Func<PartModule, IRescalable> creator)
        {
            //FIXME: user configurable setting!
            // Ctors[pm] = creator;
        }

        // Creates an updater for each modules attached to destination part.
        internal static IEnumerable<IRescalable20> CreateUpdaters(Part part)
        {
            // FIXME: Revisar isso aqui!
			IEnumerable<IRescalable20> myUpdaters = part
                .Modules.Cast<PartModule>()
                .Select(CreateUpdater)
                .Where(updater => updater != null);
            foreach (IRescalable20 updater in myUpdaters)
            {
                yield return updater;
            }
        }

        private class RescalableFacade : IRescalable20
		{
            private readonly IRescalable i;
            internal RescalableFacade (IRescalable i)
			{
                this.i = i;
			}

			public void OnRescale(ScalingFactor factor)
			{
				this.i.OnRescale(factor);
			}

			public void OnShipModified(){}
			public void OnUpdate(){}
		}
		private static IRescalable20 CreateUpdater(PartModule module)
        {
			//FIXME : This is not right! Revise!
            // ReSharper disable once SuspiciousTypeConversion.Global
			IRescalable20 updater = module as IRescalable20;
			return updater ?? (Ctors.ContainsKey(module.GetType()) ? new RescalableFacade(Ctors[module.GetType()](module)) : null);
		}

		public static IDryCost20 CreateDryCostCalcultator (Part part)
        {
            //TODO
            throw new NotImplementedException ();
        }

	}

}