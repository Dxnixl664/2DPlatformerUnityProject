using UnityEngine;

public class Carrot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Movement>().AddCarrot();
            Destroy(gameObject);
        }
    }
}
