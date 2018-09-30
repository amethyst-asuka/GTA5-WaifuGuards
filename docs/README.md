## How to spawn a Ped

```vbnet
Dim waifu As Ped = World.CreatePed(New Model(name), pos)
```

For create a ped, the ``World.CreatePed`` function required at least two parameter value: ped's model and its location in the game world. Where the peds' model can be constructed from the model name, and this model name can be customized model which is imported by using ``AddonPeds Editor`` GTAV mod.

![](./images/peds-name.png)
> You can using the ``AddonPeds Editor`` for imports your custom player skin as your waifu guards model.