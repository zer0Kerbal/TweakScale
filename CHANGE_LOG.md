# TweakScale :: Change Log

* 2015-1030: 2.2.4 (Pellinor) for KSP 0.90
	+ Fix for scaling of lists. This should fix the trouble with cost of FSFuelSwitch parts.
	+ Partial fix for editor mass display not updating
	+ new file Examples.cfg with frequently used custom patches
	+ Removed MM switch for scaleable crew pods
	+ update of NFT patches
	+ Fix scaling of resource lists
	+ support for a few missing stock parts
	+ partial support for a few other mods
	+ stock radiator support
	+ Scale ImpactRange for stock drill modules (this is what determines if the drill has ground contact or not)
	+ scale captureRange for claw (this should fix 3.75m claws not grappling)
	+ removed brakingTorque exponent (not needed and breaks stock tweakable)
* 2015-0626: 2.2.1 (Pellinor) for KSP 0.90
	+ update for KSP 1.0.4
	+ KSP 1.0 support: scaling of dragCubes
	+ exponent -0.5 for heatProduction
	+ support for HX parts from B9-Aerospace
	+ support for firespitter modules: FSEngine, FSPropellerTweak, FSAlternator
	+ remove support for KAS connector port so it stays stackable in KIS
	+ a few missing part patches
	+ update NF-Solar patches (some parts were renamed)
	+ catch exceptions on rescale
	+ survive duplicate part config
* 2015-0502: 2.1 (Pellinor) for KSP 0.90
	+ recompile for KSP 1.0.2
	+ new stock part
* 2015-0501: 2.0.1 (Pellinor) for KSP 0.90
	+ restored maxThrust exponent to fix the editor CoT display
	+ added patch for new KIS container
	+ survive mistyped scaleTypes
* 2015-0430: 2.0 (Pellinor) for KSP 0.90
	+ recompile for KSP 1.0
	+ new TWEAKSCALEBEHAVIOR nodes (engines, decouplers, boosters)
	+ scale DryCost with the mass exponent if there is no DryCost exponent defined
	+ fuel fraction of tanks is now preserved [breaking]
	+ move part patches into their own directory
	+ KIS support
	+ proper MM switches for mod exponents
	+ removed KSPI support (will be distributed with KSPI)
	+ scaleExponents for NF-electrical capacitors
	+ cleanup of stock scaleExponents
	+ support for the new stock modules
	+ support for the changed engine modules
* 2015-0420: 1.53 (Pellinor) for KSP 0.90
	+ download address for version file
	+ added missing RLA configs
	+ only touch part cost of the part is rescaled
	+ fix for repairing incomplete scaletypes
	+ support for stock decoupling modules
	+ OPT support
	+ remove RF scale exponents (RF does its own support)
* 2015-0310: 1.52.1 (Pellinor) for KSP 0.90
	+ No changelog provided
* 2015-0308: 1.52 (Pellinor) for KSP 0.90
	+ New Tweakable with more flexible intervals.
	+ All scaletypes use scaleFactors now, max/minScale is obsolete.
	+ Better handling of incomplete or inconsistent scaletype configs.
	+ Vessels now survive a change of defaultScale.
	+ less persistent data
* 2015-0226: 1.51.1 (Pellinor) for KSP 0.90
	+ added KSP-AVC support
	+ freescale slider Increments are now part of the scaletype config
	+ added stock mk3 configs
	+ auto- and chain scaling off by default (the hotkeys are leftCtrl-L and leftCtrl-K)
	+ auto- and chain scaling restricted to parts of the same scaletype
	+ Changed the 'stack' scaletype to free scaling
	+ Moved stock adapters to stack scaletype
	+ Changed surface scaletype to free scaling
	+ added an example discrete scaletype for documentation, because there is none left
	+ fixed error spam with regolith & KAS
	+ removed duplicate MM patch for IntakeRadialLong
	+ hopefully restricted the camera bug to scaled root parts
* 2015-0225: 1.51 (Pellinor) for KSP 0.90
	+ added KSP-AVC support
	+ added stock mk3 configs
		- autoscaling ==
	+ auto- and chain scaling off by default
	+ auto- and chain scaling restricted to parts of the same scaletype
	+ rewrote GetRelativeScaling based on the nodes of the prefab part
		- scaletypes ==
	+ freescale slider Increments are now part of the scaletype config
	+ Change the 'stack' scaletype to free scaling
	+ Move stock adapters to stack scaletype
	+ Change surface scaletype to free scaling
	+ added an example discrete scaletype for documentation, because there is none left in the default configs
	+ if min/maxScale are missing in a free scaletype take min/max of the scaleFactors list
		- fixes ==
	+ fixed error spam with scalable parts from KAS containers
	+ removed duplicate MM patch for IntakeRadialLong
	+ hopefully restricted the camera bug to scaled root parts
* 2014-1224: 1.50 (Biotronic) for KSP 0.24
	+ Fixed erroneous placement of attach nodes when duplicating parts.
* 2014-1218: 1.49 (Biotronic) for KSP 0.24
	+ Now saving hotkey states correctly
	+ 'Free' scaletype actually free
	+ Fixed bug in OnStart
	+ First attempt at scaling offsets
* 2014-1216: 1.48 (Biotronic) for KSP 0.24
	+ Added .90 support! (screw Curse for not having that option yet)(Admin Edit: Curse added it! File updated to reflect the proper version)
	+ Cleaned up autoscale and chain scaling!
* 2014-1117: 1.47 (Biotronic) for KSP 0.24
	+ Removed [RealChute](http://forum.kerbalspaceprogram.com/threads/57988) support
	+ Fixed a bug where TweakScale would try to set erroneous values for some fields and properties, which notably affected [Infernal Robotics](http://forum.kerbalspaceprogram.com/threads/37707)
	+ Fixed a bug where cloned fuel tanks would have erroneous volumes.
* 2014-1116: 1.46 (Biotronic) for KSP 0.24
	+ Fixed an issue where features were incorrectly scaled upon loading a ship,
	+ Scaling a part now scales its children if they have the same size.
	+ Parts now automatically guess which size they should be.
* 2014-1115: 1.45 (Biotronic) for KSP 0.24
	+ New Features:
		- Now updating UI sliders for float values.
		- Better support for [KSP Interstellar](http://forum.kerbalspaceprogram.com/threads/43839).
		- Added support for [TweakableEverything](http://forum.kerbalspaceprogram.com/threads/64711).
		- Added support for [FireSpitter](http://forum.kerbalspaceprogram.com/threads/24551)'s FSFuelSwitch.
* 2014-1010: 1.44 (Biotronic) for KSP 0.24
	+ Version 1.44:
		- Updated for KSP 0.25
		- Added ability to not update certain properties when a specific partmodule is on the part.
	+ Thanks a lot to NathanKell, who did all the work on this release!
* 2014-0824: 1.43 (Biotronic) for KSP 0.24
	+ Version 1.43:
		- Added licence file (sorry, mods!)
		- No longer chokes on null particle emitters.
* 2014-0815: 1.41 (Biotronic) for KSP 0.24
	+ Version 1.41:
		- Fixed scaling of Part values in unnamed TWEAKSCALEEXPONENTS blocks.
* 2014-0813: 1.40 (Biotronic) for KSP 0.24
	+ Version 1.40:
		- Removed [Karbonite](http://forum.kerbalspaceprogram.com/threads/89401) cfg, since that project does its own TweakScale config.
* 2014-0812: 1.39 (Biotronic) for KSP 0.24
	+ Version 1.39:
		- Fixed cost calculation for non-full tanks.
* 2014-0812: 1.38 (Biotronic) for KSP 0.24
	+ Version 1.38:
		- Added scaling of FX.
		- Added support for [Banana for Scale](http://forum.kerbalspaceprogram.com/threads/89570).
		- Updated [Karbonite](http://forum.kerbalspaceprogram.com/threads/87335) support.
		- Fixed a bug where no scalingfactors available due to tech requirements would lead to unintended consequences.
* 2014-0805: 1.37 (Biotronic) for KSP 0.24
	+ Version 1.37:
		- Updated cost calculation.
* 2014-0804: 1.36 (Biotronic) for KSP 0.24
	+ Version 1.36:
		- Updated [Real Fuels](http://forum.kerbalspaceprogram.com/threads/64118) and [Modular Fuel Tanks](http://forum.kerbalspaceprogram.com/threads/64117) support.
		- Added [KSPX](http://forum.kerbalspaceprogram.com/threads/30472) support.
* 2014-0803: 1.35 (Biotronic) for KSP 0.24
	+ Corrected cost calculation.
	+ Updated to [KSPAPIExtensions 1.7.0](http://forum.kerbalspaceprogram.com/threads/81496)
* 2014-0802: 1.34 (Biotronic) for KSP 0.24
	+ Version 1.34:
		- Fixed a bug where repeated scaling led to inaccurate placing of child parts.
		- Added [Karbonite](http://forum.kerbalspaceprogram.com/threads/87335) support.
* 2014-0728: 1.33 (Biotronic) for KSP 0.24
	+ Updated RealFuels support for 7.1
* 2014-0725: 1.32 (Biotronic) for KSP 0.24
	+ Version 1.32:
		- Updated KSPAPIExtension for 0.24.2 support.
* 2014-0725: 1.31 (Biotronic) for KSP 0.24
	+ Fixed a bug where parts with defaultScale inaccessible due to tech requirements were incorrectly scaled.
* 2014-0725: 1.30 (Biotronic) for KSP 0.24
	+ Updated KSPAPIExtensions with 24.1 support.
	+ Re-enabled Real Fuels support.
	+ Added support for IPartCostModifier.
* 2014-0724: 1.29 (Biotronic) for KSP 0.24
	+ Fixed Modular Fuel Tanks support.
* 2014-0724: 1.28 (Biotronic) for KSP 0.24
	+ Fixed more cross-platform bugs.
	+ Added [Tantares Space Technologies](http://forum.kerbalspaceprogram.com/threads/80550).
* 2014-0723: 1.27 (Biotronic) for KSP 0.24
	+ Fixed a bug on non-Windows platforms.
* 2014-0723: 1.26 (Biotronic) for KSP 0.24
	+ Version 1.26:
		- Fixed typo in DefaultScales.cfg that caused som parts to baloon ridiculously.
		- Added support for updated NFT and KW Rocketry
* 2014-0723: 1.25 (Biotronic) for KSP 0.24
	+ Version 1.25:
		- Modular Fuel Tanks](http://forum.kerbalspaceprogram.com/threads/64117) yet again supported! (Still waiting for Real Fuels)
		- Refactored IRescalable system to be easier for mod authors.
		- Fixed a bug where one field could have multiple exponents, and thus be rescaled multiple times.
* 2014-0721: 1.23 (Biotronic) for KSP 0.24
	+ Version 1.23:
		- Duplicate TweakScale dlls no longer interfere.
* 2014-0720: 1.22 (Biotronic) for KSP 0.24
	+ Version 1.22
		- Fixed tanks that magically refill.
		- Fixed technology requirements are too slow.
		- Updated KSPAPIExtensions to make TweakScale play nicely with other mods.
* 2014-0718: 1.21 (Biotronic) for KSP 0.24
	+ Version 1.21:
		- Updated for 0.24
		- Now supports global, per-part, and per-scaletype scaling of features (like mass, buoyancy, thrust, etc)
* 2014-0613: 1.20 (Biotronic) for KSP 0.23.5
	+ Version 1.20:
		- New algorithm for rescaling attach nodes. Tell me what you think!
		- Added [Deadly Reentry Continued](http://forum.kerbalspaceprogram.com/threads/54954) and [Large Structural/Station Components](http://forum.kerbalspaceprogram.com/threads/34664).
* 2014-0606: 1.19 (Biotronic) for KSP 0.23.5
	+ Version 1.19:
		- Added support for tech requirements for non-freescale parts.
* 2014-0606: 1.18 (Biotronic) for KSP 0.23.5
	+ Version 1.18
		- Factored out Real Fuels and Modular Fuel Tanks support to separate dlls.
* 2014-0603: 1.17 (Biotronic) for KSP 0.23.5
	+ Version 1.17:
		- Fixed bug where attachment nodes were incorrectly scaled after reloading. This time with more fix!
		- Added support for [Near Future Technologies](http://forum.kerbalspaceprogram.com/threads/52042).
* 2014-0603: 1.16 (Biotronic) for KSP 0.23.5
	+ Version 1.16:
		- Fixed bug where attachment nodes were incorrectly scaled after reloading.
* 2014-0603: 1.15 (Biotronic) for KSP 0.23.5
	+ Version 1.15:
		- Finally squished the bug where crafts wouldn't load correctly. This bug is present in 1.13 and 1.14, and affects certain parts from Spaceplane+, MechJeb, and KAX.
* 2014-0603: 1.14 (Biotronic) for KSP 0.23.5
	+ Version 1.14:
		- Fixed a bug where nodes with the same name were moved to the same location regardless of correct location. (Only observed with KW fairing bases, but there could be others)
* 2014-0602: 1.13 (Biotronic) for KSP 0.23.5
	+ Version 1.13:
		- Added support for [MechJeb](http://forum.kerbalspaceprogram.com/threads/12384), [Kerbal Aircraft eXpanion](http://forum.kerbalspaceprogram.com/threads/76668), [Spaceplane+](http://forum.kerbalspaceprogram.com/threads/80796), [Stack eXTensions](http://forum.kerbalspaceprogram.com/threads/79542), [Kerbal Attachment System](http://forum.kerbalspaceprogram.com/threads/53134), [Lack Luster Labs](http://forum.kerbalspaceprogram.com/threads/24906), [Firespitter](http://forum.kerbalspaceprogram.com/threads/24551), [Taverio's Pizza and Aerospace](http://forum.kerbalspaceprogram.com/threads/15348), [Better RoveMates](http://forum.kerbalspaceprogram.com/threads/75873), and [Sum Dum Heavy Industries Service Module System](http://forum.kerbalspaceprogram.com/threads/48357).
		- Fixed a bug where Modular Fuel Tanks were not correctly updated.
* 2014-0602: 1.12 (Biotronic) for KSP 0.23.5
	+ Version 1.12:
		- Added support for [КОСМОС](http://forum.kerbalspaceprogram.com/threads/24970).
		- No longer scaling heatDissipation, which I was informed was a mistake.
* 2014-0601: 1.11 (Biotronic) for KSP 0.23.5
	+ 1.11:
		- Removed silly requirement of 'name = *' for updating all elements of a list.
		- Added .cfg controlled scaling of Part fields.
* 2014-0601: 1.10 (Biotronic) for KSP 0.23.5
	+ 1.10:
		- Added support for nested fields.
* 2014-0531: 1.9 (Biotronic) for KSP 0.23.5
	+ Version 1.9
		- Fixed bugs in 1.8 where duplication of parts caused incorrect scaling.
* 2014-0530: 1.8 (Biotronic) for KSP 0.23.5
	+ Version 1.8:
		- Fixed a bug where rescaleFactor caused incorrect scaling.
		- Added (some) support for [Kethane](http://forum.kerbalspaceprogram.com/threads/23979-Kethane-Pack-0-8-5-Find-it-mine-it-burn-it!-0-23-5-\(ARM\)-compatibility-update) parts.
* 2014-0522: 1.7 (Biotronic) for KSP 0.23.5
	+ No changelog provided
* 2014-0522: 1.6 (Biotronic) for KSP 0.23.5
	+ Version 1.6:
		- Fixed a problem where parts were scaled back to their default scale after loading, duplicating and changing scenes.
* 2014-0520: 1.5.0.1 (Biotronic) for KSP 0.23.5
	+ Version 1.5.0.1:
		- Fixed a bug in 1.5
		- Changed location of KSPAPIExtensions.dll
* 2014-0520: 1.5 (Biotronic) for KSP 0.23.5
	+ Version 1.5
		- Changed from hardcoded updaters to a system using cfg files.
* 2014-0520: 1.4 (Biotronic) for KSP 0.23.5
	+ Version 1.4
		- Fixed incompatibilities with GoodspeedTweakScale
* 2014-0519: 1.3 (Biotronic) for KSP 0.23.5
	+ Version 1.3
		- Fixed a bug where parts would get rescaled to stupid sizes after loading.
		- Breaks compatibility with old version of the plugin (pre-1.0) and GoodspeedTweakScale. :(
* 2014-0518: 1.2 (Biotronic) for KSP 0.23.5
	+ Version 1.2 (2014-05-18, 22:00 UTC):
		- Fixed default scale for freeScale parts.
		- Fixed node sizes, which could get absolutely redonkulous. Probably not perfect now either.
		- B9, Talisar Cargo Transportation Solutions, and NASA Module Manager configs.
		- Now does scaling at onload, removing the problem where the rockets gets embedded in the ground and forcibly eject at launch.
		- Fixed a silly bug in surface scale type.
* 2014-0516: 1.1 (Biotronic) for KSP 0.23.5
	+ Version 1.1:
		- Added scaling support for [B9 Aerospace ](http://forum.kerbalspaceprogram.com/threads/25241)and [Talisar's Cargo Transportation Solutions](http://forum.kerbalspaceprogram.com/threads/77505)
		- Will now correctly load (some) save games using an older version of the plugin.
* 2014-0516: 1.0 (Biotronic) for KSP 0.23.5
	+ No changelog provided
