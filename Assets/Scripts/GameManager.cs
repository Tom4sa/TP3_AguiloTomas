using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controla victoria, derrota, objetivos intermedios, llaves y enemigos vivos.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Health player;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Objetivos")]
    public int objectivesRequired = 1;        // ej: activar el interruptor
    public bool requireAllEnemiesDead = false;

    public int ObjectivesDone { get; private set; }
    public int EnemiesAlive { get; private set; }
    public bool GameEnded { get; private set; }

    readonly HashSet<string> keys = new HashSet<string>();

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        player.OnDeath += Lose;
    }

    void Update()
    {
        if (GameEnded && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegisterEnemy() { EnemiesAlive++; }
    public void EnemyDied() { EnemiesAlive--; }

    public void CompleteObjective() { ObjectivesDone++; }
    public void AddKey(string id) { keys.Add(id); }
    public bool HasKey(string id) { return keys.Contains(id); }

    public bool CanExit()
    {
        if (ObjectivesDone < objectivesRequired) return false;
        if (requireAllEnemiesDead && EnemiesAlive > 0) return false;
        return true;
    }

    public string ObjectiveText()
    {
        if (ObjectivesDone < objectivesRequired)
            return $"Objetivos: {ObjectivesDone}/{objectivesRequired}";
        if (requireAllEnemiesDead && EnemiesAlive > 0)
            return $"Elimina a los enemigos: {EnemiesAlive}";
        return "Llega a la salida";
    }

    public void Win()
    {
        if (GameEnded) return;
        EndGame(winPanel);
    }

    void Lose()
    {
        if (GameEnded) return;
        EndGame(losePanel);
    }

    void EndGame(GameObject panel)
    {
        GameEnded = true;
        if (panel != null) panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
