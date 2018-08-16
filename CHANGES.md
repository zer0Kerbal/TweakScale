# TweakScale :: Changes

* 2018-0816: 2.3.12.1 (Lisias) for KSP 1.4.x
	+ Saving xml config files under <KSP_ROOT>/PluginData Hierarchy
		- Added hard dependency for [KSP API Extensions/L](https://github.com/net-lisias-ksp/KSPAPIExtensions). 
	+ Removed deprecated DLLs
		- Needs TweakableEverything installed now
		- A small hack:
			- one DLL was moved to a new Plugin directory inside the dependency to overcome the loading order problem.
			- a better solution is WiP for the next release
	+ Removed Support code for deleted KSP functionalities
		- Not needed anymore? (RiP) 
* 2018-0416: 2.3.12 (Pellinor) for KSP 1.4.2
	+ configs for new parts
	+ fix for exceptions
	+ fix for solar panels
	+ recompile for KSP 1.4.2

- - -
	WiP : Work In Progress
	RiP : Research In Progress
