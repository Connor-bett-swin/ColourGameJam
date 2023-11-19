
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class EmmiterLoadNextScene : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
