using System;
using UnityEngine;

/// <summary>
/// Central event hub (Observer pattern). GameManager, CameraEffects, enemies
/// and the player broadcast/react to these instead of holding direct
/// references to each other. This is what removes the GameObject.Find calls
/// and lets any of these systems change independently (Dependency Inversion +
/// Open/Closed: add a new listener without editing the broadcaster).
/// </summary>
public static class GameEvents
{
    /// <summary>Fired whenever ANY enemy (meteor, big meteor, orbiting enemy) dies. Used for camera shake.</summary>
    public static event Action<Vector3> OnEnemyDestroyed;

    /// <summary>Fired only when a small Meteor dies. GameManager counts these toward spawning a BigMeteor.</summary>
    public static event Action OnMeteorDestroyed;

    /// <summary>Fired when a BigMeteor spawns. CameraEffects zooms out briefly.</summary>
    public static event Action OnBigMeteorSpawned;

    /// <summary>Fired when the player dies. GameManager sets game-over state.</summary>
    public static event Action OnPlayerDied;

    public static void EnemyDestroyed(Vector3 position) => OnEnemyDestroyed?.Invoke(position);
    public static void MeteorDestroyed() => OnMeteorDestroyed?.Invoke();
    public static void BigMeteorSpawned() => OnBigMeteorSpawned?.Invoke();
    public static void PlayerDied() => OnPlayerDied?.Invoke();
}
