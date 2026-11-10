using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    private int score;

    public Image fadeImage;

    private Blade blade;
    private Spawner spawner;

    private void Awake()
    {
        blade = FindAnyObjectByType<Blade>();
        spawner = FindAnyObjectByType<Spawner>();
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        Time.timeScale = 1f;

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();

        ClearScene();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequenceRoutine());
    }

    private IEnumerator ExplodeSequenceRoutine()
    {
        float elapsed = 0;
        float duration = 0.5f;

        while(elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1 - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);

        NewGame();

        elapsed = 0;

        while(elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsByType<Fruit>();
        
        foreach(Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsByType<Bomb>();
        
        foreach(Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }

}
