using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeSystem : MonoBehaviour
{
    public int lives = 3;

    private LivesController livesController;
    private Vector3 spawnPosition;
    private Movement playerMovement;
    private void Start()
    {
        spawnPosition = transform.position;
        livesController = GameObject.Find("LivesController").GetComponent<LivesController>();
        livesController.UpdateLivesDisplay(lives);

        playerMovement = GetComponent<Movement>();
        playerMovement.livesText.text = lives.ToString();
    }

    public void LoseLife()
    {
        lives--;
        livesController.UpdateLivesDisplay(lives);

        if(playerMovement != null)
        {
            playerMovement.livesText.text = lives.ToString();
        }
        else{
            Debug.Log("Movement component not found");
        }

        transform.position = spawnPosition;

        if (lives <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}