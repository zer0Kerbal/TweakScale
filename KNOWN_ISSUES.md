# TweakScale :: Known Issues

* TweakScale 2.4.x is known to (purposely) withdraw support for some parts on runtime. This, unfortunately, damages crafts at loading (including from flying ones) as the TweakScale data plain vanishes and the part goes back to stock.
	+ Parts being deactivated are being logged into KSP.log, pinpointing to an URL where the issue it causes is described. TweakScale **does not** hides from you what it's being done.
	+ This is unavoidable, unfortunately, as the alternative is a fatal corruption of the game state (persisted on savegames) that leads to blowing statics and ultimately game crash.
	+ The Maintainer is terribly sorry for the mess (my savegames gone *kaput* too), but it's the less evil of the available choices.
	+ The proposed mitigation measure is to backup your savegames, try TweakScale 2.4.x and then decide if the damages (if any, only a few parts are affected) are bigger than the risks - but then, make **hourly** backups of your savegames as one the misbehaviour is triggered, your savegame can be doomed and forever leading to crashes.
	+ Related issues:
		- [#9](https://github.com/net-lisias-ksp/TweakScale/issues/9) Weird issue with SXT parts using FSBuoyancy
		- [#11](https://github.com/net-lisias-ksp/TweakScale/issues/11) Negative mass on parts
		- [#12](https://github.com/net-lisias-ksp/TweakScale/issues/12) Zero Mass on Parts
		- [#15](https://github.com/net-lisias-ksp/TweakScale/issues/15) Prevent B9PartSwitch to be handled when another Part Switch is active

- - -

* RiP : Research in Progress
* WiP : Work in Progress
