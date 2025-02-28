# Bullet-Hell Game Task Test

This project presents my solution to a test task for a Unity mobile developer position.

## Task Description

Duration : 1 week

### Stack :
- MVP pattern
- Zenject DI
- UniRx
- UniTask
- DoTween

### Game Description:

The game follow the base principe of Bullet-Hell genra.

#### Enemies

Enemies spawn off-screen every X seconds, then move toward the player.
Enemies respawn when out of sight.
Enemies damage the player when in range, the player can survive more hit than enemies.

#### Weapon

Weapon shoot projectile every X seconds in a random direction.
Projectile ricochet on the screen edges, "snapping" toward an enemy on bounce.

#### UI

Player and enemies have health bar indicating the current health.
HUD must display a counter keeping track of the amount of enemies defeated so far.

## Architecture

### Weapons
The WeaponBelt acts as a holder of the game weapon firing logic, it triggers the fire of each weapon once it's cooldown have passed.

The base weapon logic (firing loop) is handled in the base `WeaponPresenter` class from which each weapon must extend.
Each weapon specific logic is implemented in the presenter (including spawning specific projectiles) and the projectile presenter (ex: bounce logic for Bolt projectile).

### Enemy Spawner
The Enemy Spawner acts as a holder of enemy spawn and respawn logic, it checks at specific interval what enemies must be respawned and select random location for each enemy to spawn.
Each enemy spawned is autonomous (chase the player, attack the player)

## Configuration
`GameSettings` contains all the settings of the game : 
- **Editor Draw Line** : toggle the editor display (_Debug.DrawLine_) of the aimbot logic for the bolt ricochet.
- **Player Settings** : Player starting _Health_, _AttackSpeed_ and _MoveSpeed_
- `Enemy Spawner Settings` :
  - **Start Enemy Count** : initial spawn of enemies at the start of a new game.
  - **Max Enemy Count** : maximum spawned enemies at any point in the game.
  - **Enemy Spawn Rate** : amount of enemy spawned per second.
  - **Enemy Spawn Margin** : external margin from the screen border from which enemies are spawned.
  - **Respawn Check Inverval** : time interval between each check for enemies that must be respawned.
  - **Respawn Margin** : external margin from the screen border beyond which enemies are respawned.
    > must be higher than the spawn margin or enemies might be constantly respawned, though this can be mitigated from the respawn check interval.
  - `Enemy Settings` : insert an _EnemySettings_ preset.
    - **Prefab** : prefab of the enemy.
    - **Data** : Enemy starting _Health_ and _MoveSpeed_
    - **Attack** : Enemy attack _Range_, _Speed_ (attack per seconds) and _Damage_
- `Weapon Settings` : insert an _WeaponSettings_ preset
  - **Cooldown** : time (in seconds) between each fire of the weapon
  - **Burst Count** : amount of projectiles for each fire of the weapon
  - **Burst Interval** : amount of time between each projectile in a burst
  - `Projectile Settings` : insert an _ProjectileSetting_ preset
    - **Prefab** : prefab of the projectile
    - Projectile _Speed_, _Lifetime_ (duration in second) and _Damage_
    - [BOLT] **Bounce Aimbot Factor** : The amount of freedom choosing an enemy not aligned with the bounce direction.
    
  
## Extras

### Bounce logic
The bounce bias to snap at a enemy direction can be configured in the Bolt projectile settings :
A value of 0 mean there is no aim assist in the bounce, aka the realistic bounce. 
A value of 1 mean the projectile can snap to any enemy in a 180 angle ahead of it from its bounce direction.

Enemies behind the projectile (based on bounce direction) are not considered legal targets for the bias.

The bounce always targets the enemy the most aligned to the original bouncing direction, so the bounce remains as realistic as possible.
The logic can be displayed in the editor scene by toggling the GameSettings `Editor Draw Line`.
- White line displays the default bounce direction (realistic one)
- Yellow lines display the available targets for snap based on the `Bounce Aimbot Factor` of the projectile.
- Blue line displays the final bounce direction.

### Background
The background moves with the player, snapping to positions matching its tiling size, so its invisible to the user.
This effectively simulates infinite map to the player.

### Player Death
On player death, all enemies stop chasing or attacking the player, and all active projectiles are removed.
A small UI will allow the user to choose between ``Revive`` or ``New Game``
- ``Revive``: The enemies are pushed up to 5 units away from the player and the player life is restored.
- ``New Game``: The enemies are removed and the game data is reseted (timer, kill counter, etc).

### Extending the game

The game implements most basic Bullet-Hell data common in the genra (like global _attack speed_, _projectiles per burst_, _projectile speed_, etc) in anticipation of adding boost "tomes" and power up.
It is also possible to create new weapons to the player by extending base classes and installer for the weapon and projectiles.

### Joystick
The Joystick have been enhanced to be usable anywhere on the screen, as on Heroes vs. Hordes.

