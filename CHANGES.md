# TweakScale :: Changes

* 2019-0216: 2.4.1.0 (Lisias) for KSP >= 1.4.1
	+ Adding 1.875 scale as default (being now a Stock size on MH, it makes sense to properly acknowledge it). Suggested by Tyko.
		- Closing issue [#3](https://github.com/net-lisias-ksp/TweakScale/issues/3)
	+ Adding support for Stock Alike Station Parts. Courtesy of Speadge.
		- Closing issue [#8](https://github.com/net-lisias-ksp/TweakScale/issues/8)
	+ Fixed a critical craft corruption (even flying ones) as TweakScale is sometimes being injected twice (or even more) into a part. This patch does not fix the duplicity, but prevent your crafts from being corrupted once a fix is applied (yeah - fixing the bug would cause craft corruption without this patch!)
		- Closing issue [#20](https://github.com/net-lisias-ksp/TweakScale/issues/20)
