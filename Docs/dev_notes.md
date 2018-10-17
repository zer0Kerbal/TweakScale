# Development Notes

* from Pellinor:
	 * scale_redist:
	 	* this is the API definition of TweakScale. Other mods can include this in their distribution and compile against it without depending on a TweakScale version. It does not depend on KSP and has not been touched since biotronic wrote it. So I think its version should stay at 1.0, to make clear that other mods do not need to compile against a new version﻿.
	* Considering the difficulties of TweakScale:
		* scaling a part was always the easy part, what can get messy is how to change all the relevant properties according to the scale. Which means manipulating fields in lots of other mods, and have them used (and abused) in all sorts of ways by the playerbase. TweakScale had a reputation of breaking other mods in the past, but I think things are quite calm and stable now.
		* Most of the maintenance comes from updating MM patches, e,g, when new stock parts appear. And occasionally there is a new part module that needs scaling.
		* Most of the complexity is in the configuration interface, with its nested definitions of scaling exponents and an unknown number of legacy MM patches distributed with other mods. This is why I have been reluctant to add more complexity at that front.
	* I see two extremes of players. The artists who want to scale everything to any size, often for purely aesthetic purposes. And the career players who want something that is "cheat-safe" in a stock career setting. I have never been a big fan of restrictions, so the default configuration is more on the artists side.

* from Lisias:
	* tweakableEverything﻿:
		* We need to contact these guys and ask how they synchronize with us. Should we keep providing the DLL on the package? 
	* orthodox branch:
		* all "consumer safe" development is done on this branch, and then merged into the heterodox branch
		* It's promoted as "master" once a release is delivered.
	* heterodox branch:
		* where crazy and sometimes stupid ideas can be tested. *NEVER* merged back to orthodox.
			* In the event an idea proves to be valid *and* safe, it will be cherry-picked.
			* Yeah, I'm paranoid. :) 
