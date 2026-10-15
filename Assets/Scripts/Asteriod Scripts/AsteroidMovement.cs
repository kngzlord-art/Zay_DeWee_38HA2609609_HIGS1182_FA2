using Unity.VisualScripting;
using UnityEngine;

public class AsteroidAI : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;


    private Transform player;

    private void Start()
    {

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        Movement();
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }
}
