using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public static event Action OnEnemyKilled;

    private void KillEnemy(GameObject enemy)
    {
        Destroy(enemy);
        OnEnemyKilled?.Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                KillEnemy(other.gameObject);
                break;
        }
    }
}