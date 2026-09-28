using System;
using UnityEngine;


public static class GameEvents
{
    //Fired whenever meteors die for camera shake
    public static event Action<Vector3> OnEnemyDestroyed;
    //Fired only when a small Meteor dies
    public static event Action OnMeteorDestroyed;
    //Fired when a BigMeteor spawns 
    public static event Action OnBigMeteorSpawned;
    //Fired when the player dies
    public static event Action OnPlayerDied;
    public static void EnemyDestroyed(Vector3 position) => OnEnemyDestroyed?.Invoke(position);
    public static void MeteorDestroyed() => OnMeteorDestroyed?.Invoke();
    public static void BigMeteorSpawned() => OnBigMeteorSpawned?.Invoke();
    public static void PlayerDied() => OnPlayerDied?.Invoke();
}
