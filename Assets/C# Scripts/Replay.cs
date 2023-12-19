using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    public void ReplayButton()
    {
        SceneManager.LoadScene("Start Scene");
    }
}