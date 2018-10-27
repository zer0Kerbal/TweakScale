# TweakScale :: Known Issues

* There're some *Unholy Interactions between Add-Ons*, still RiP, that lead to the following errors:
	+ Exceptions thrown by TweakScale while trying to calculate the DryMass of the parts used to implement Kerbals doing EVA. See note (1)
		- These Exceptions can be safely ignored.
	+ Parts using B9PartsSwitch causing negative DryCost. See note (2)
		- **This is a serious showstopper**
			- A third add-on (what is RiP) somehow is inducing B9PartsSwitch parts to have all their properties set to Zero.
			- This renders crafts using such parts **unfliable**, with nasty collateral effects on some statics on the scene.
		- If you note such Exceptions on your KSP.log, please file a report attaching it.
	
	
Note (1): 
```
[ERR 18:19:54.353] [TweakScale] ERROR: [TweakScale] Exception on kerbalEVA.prefab.Modules.Contains: System.NullReferenceException: Object referenc
  at PartModuleList.Contains (Int32 classID) [0x00000] in <filename unknown>:0
  at PartModuleList.Contains (System.String className) [0x00000] in <filename unknown>:0
  at TweakScale.PrefabDryCostWriter+<WriteDryCost>d__4.MoveNext () [0x00000] in <filename unknown>:0

[ERR 18:19:54.355] [TweakScale] ERROR: [TweakScale] Exception on kerbalEVAfemale.prefab.Modules.Contains: System.NullReferenceException: Object re
  at PartModuleList.Contains (Int32 classID) [0x00000] in <filename unknown>:0
  at PartModuleList.Contains (System.String className) [0x00000] in <filename unknown>:0
  at TweakScale.PrefabDryCostWriter+<WriteDryCost>d__4.MoveNext () [0x00000] in <filename unknown>:0
``` 

Note (2):
```
[ERR 18:19:54.358] [TweakScale] ERROR: TweakScale::PrefabDryCostWriter: negative dryCost: part=B9.Adapter.SM1, DryCost=-11.415
[ERR 18:19:54.359] [TweakScale] ERROR: TweakScale::PrefabDryCostWriter: negative dryCost: part=B9.Adapter.SM2, DryCost=-15.435
[ERR 18:19:54.361] [TweakScale] ERROR: TweakScale::PrefabDryCostWriter: negative dryCost: part=B9.Adapter.LM3, DryCost=-27.8005
[ERR 18:19:54.363] [TweakScale] ERROR: TweakScale::PrefabDryCostWriter: negative dryCost: part=B9.Adapter.SM3, DryCost=-13.7475
[ERR 18:19:54.365] [TweakScale] ERROR: TweakScale::PrefabDryCostWriter: negative dryCost: part=B9.Aero.HL.Adapter.Front, DryCost=-2063.875
--- and many more!! ---
```

- - -

* RiP : Research in Progress
* WiP : Work in Progress
