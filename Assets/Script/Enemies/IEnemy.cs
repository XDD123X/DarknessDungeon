using UnityEngine;

interface IEnemy {
    public void Spawn();
    public void Attack();
    public void Move(Vector2 move);
    public void Dead();
    
}
