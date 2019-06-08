# TweakScale :: Changes

* 2019-0608: 2.4.3.0 (Lisias) for KSP >= 1.4.1
	+ This is an emergencial Release due a Show Stopper issue (see Issue #34 below) with some new features.
	+ Adding features:
		- [#7](https://github.com/net-lisias-ksp/TweakScale/issues/7) Adding support for new Parts from KSP 1.5 and 1.6 (and Making History)! (**finally!**)
		- [#35](https://github.com/net-lisias-ksp/TweakScale/issues/35) Checking for new Parts on KSP 1.7 (none found)
			- (Serenity is Work In Progress)
		- Adding KSPe.Light support for some UI features. 
	+ Fixing bugs:
		- [#31](https://github.com/net-lisias-ksp/TweakScale/issues/31) Preventing being ran over by other mods
		- [#34](https://github.com/net-lisias-ksp/TweakScale/issues/34) New Sanity Check: duplicated properties
	+ [Known Issues](https://github.com/net-lisias-ksp/TweakScale/blob/master/KNOWN_ISSUES.md) update:
		- A new and definitively destructive interaction was found due some old or badly written patches ends up injecting TweakScale properties **twice** on the Node.
