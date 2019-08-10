## ANNOUNCE

Release 2.4.3.1 is available for downloading, with the following changes:

+ This is an emergencial Release due a Emergencial Release. :P
+ Adding KSPe Light facilites:
	- Logging
+ Closing or reworking the following issues:
	- [#31](https://github.com/net-lisias-ksp/TweakScale/issues/31) Preventing being ran over by other mods
		- A misbehaviour on detecting the misbehaviour :) was fixed.
	- [#47](https://github.com/net-lisias-ksp/TweakScale/issues/47) Count failed Sanity Checks as a potential problem. Warn user.
	- [#48](https://github.com/net-lisias-ksp/TweakScale/issues/48) Backport the Heterodox Logging system into Orthodox (using KSPe.Light)
	- [#49](https://github.com/net-lisias-ksp/TweakScale/issues/49) Check the Default patches for problems due wildcard!
	- [#50](https://github.com/net-lisias-ksp/TweakScale/issues/50) Check the patches for currently supported Add'Ons
		- ModuleGeneratorExtended Behaviour
	- [#51](https://github.com/net-lisias-ksp/TweakScale/issues/51) Implement a "Cancel" button when Actions are given to MessageBox
		- Yeah. Doing it right this time.
	- [#54](https://github.com/net-lisias-ksp/TweakScale/issues/54) [ERR ***FATAL*** link provided in KSP.log links to 404
		- "Typo maldito, typo maldito - tralálálálálálá"
	- [#56](https://github.com/net-lisias-ksp/TweakScale/issues/56) "Breaking Parts" patches
	- [#57](https://github.com/net-lisias-ksp/TweakScale/issues/57) Implement Warning Dialogs
		- Warnings about Overrules, parts that couldn't be checked and parts with TweakScale support withdrawn.
		- Doing it right this time!
	- [#58](https://github.com/net-lisias-ksp/TweakScale/issues/58) Mk4 System Patch (addendum)

See OP for the links.

## Highlights

A new TWEAKSCALEBEHAVIOUR, ModuleGeneratorExtended , is available for parts using ModuleGenerator that wants to scale the INPUT_RESOURCES too. This feature wasn't introduced directly into the ModuleGenerator's TWEAKSCALEEXPONENTS to prevent damage on Add'Ons (and savegames) that rely on the current behaviour (scaling only the output), as suddenly the resource consumption would increase on already stablished bases and crafts.

Just add the lines as the example below (the output resources scaling is still inherited from the default patch!).

```
@PART[my_resource_converter]:NEEDS[TweakScale]
{
    #@TWEAKSCALEBEHAVIOR[ModuleGeneratorExtended]/MODULE[TweakScale] { }
    %MODULE[TweakScale]
    {
        type = free
    }
}
```

## WARNINGS

The last detected *Unholy interaction between modules* (Kraken Food), when rogue patches apply twice the same property on a part, are still detected on the Sanity Checks and a (now) proper (scaring) warning is being shown. Unfortunately, this issue is a serious Show Stopper, potentially (and silently) ruining your savegames. This is not TweakScale fault, but yet it's up to it to detect the problem and warn you about it. If this happens with you, call for help. Now a "Cancel" button is available for the brave Kerbonauts willing to fly unsafe.

TweakScale strongly recommends using [S.A.V.E.](https://forum.kerbalspaceprogram.com/index.php?/topic/94997-*).

Special procedures for recovering mangled installments once the TweakScale are installed (triggering the MM cache rebuilding) are possible, but **keep your savegames backed up**. And **DON`T SAVE** your crafts once you detect the problem. Reach me on [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/179030-*) for help.

TweakScale stills "mangles further" affected crafts and savegames with some badly (but recoverable) patched parts so when things are fixed, your crafts preserve the TweakScale settings without harm. **THIS DOES NOT FIX THE PROBLEM**,  as this is beyond the reach of TweakScale - but it at least prevents you from losing your crafts and savegames once the problem happens and then is later fixed.

As usual, this version still drops support in runtime for some problematic parts. Any savegame with such problematic parts scaled will have them "descaled". This is not a really big problem as your game was going to crash sooner or later anyway - but if you plan to return to such savegame later when TweakScale will fully support that parts again, it's better to backup your savegames!

Keep an eye on the [Known Issues](https://github.com/net-lisias-ksp/TweakScale/blob/master/KNOWN_ISSUES.md) file.

— — — — —

This Release will be published using the following Schedule:

* GitHub, reaching first manual installers and users of KSP-AVC. Right now.
* CurseForge, by Saturday night
* SpaceDock (and CKAN users), by Sunday night.

The reasoning is to gradually distribute the Release to easily monitor the deployment and cope with eventual mishaps.
