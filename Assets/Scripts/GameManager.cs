using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private float gameTime = 60f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            gameTime -= Time.deltaTime;
            if (gameTime <= 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("¡Has sobrevivido!"); // Puedes reemplazar con una UI
    }
}