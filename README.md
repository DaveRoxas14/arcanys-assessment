# Arcanys Technical Assessment by Dave Louie Roxas

## Adding new Gem type
### Prefab Preparation
- Create a new prefab variant from `Score Gem.Prefab` in `Game/Prefab`
- Replace the model and vfx to anything you want in the hierarchy.
- On the `Gem` component, reference the model and vfx. You can also change the `SFX` and `Effect Behavior` of this specific gem
### Effect Behavior setup
- You can either use the existing `EffectBehavior` scriptable objects or you can make it youself.
- Create a new script that inherits `CollectibleEffectSo` and implement the missing members.
- Under the `UseCollectible` function you can freely add a logic that will be triggered once the gem is collected by the player.

```c#
// "So" means Scriptable Object
[CreateAssetMenu(menuName = "Gem/Powerup Gem")]
public class PowerupGemSo : CollectibleEffectSo
    {
        [SerializeField] private int duration = 3;
        
        public override void UseCollectible(PlayerController collector)
        {
           // powerup enemy for set duration
        }
    }
```

- Create the effect scriptable object asset that you just made. In my example above, it's in `Create -> Gem -> Powerup Gem`
- After that you can now reference that effect behavior to the `Gem` component.
- You can now reference the new gem prefab into any `CollectibleSpawner.cs` in the game area. Just add the reference to `Collectible Prefabs`.


## Adding new enemy type
- Create a new enemy type script and inherit `EnemyBase.cs`.
- Implement missing members.
- You can now override the `Move()` and `Attack()` methods, and even change the attack condition.
- You can create a new prefab variant of `Normal Enemy` in `Game/Prefab` or just create one yourself. As long as you replace the `Normal Enemy.cs` script with the new enemy type you just created.

```c#
public class NewEnemy : EnemyBase
    {
        protected override void Attack()
        {
            // set attack logic here...
        }
        
        protected override void Move()
        {
            base.Move();
            // change movement.. make them skate or fly if you want...
        }
        
        protected override void AttackWhenInDistance()
        {
            // you can also change the attack condition here...
        }
    }
```

## Credits
- Model & Animation: Little Heroes Mega Pack by Meshtint Studio
- VFX: Cartoon FX Remaster by Jean Moreno
- Music: Ultimate Game Music Collection by John Leonard French
- SFX: Universal Sound FX by Imphenzia
