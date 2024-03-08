using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
