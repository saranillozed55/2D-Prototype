using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public int health;
    public bool isDead = false;
    public float playerRespawnTime = 3f;
    //public float moveSpeed = 5f;
    //public float sprintSpeed = 8f;
}
