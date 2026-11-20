using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private float duration = 2.5f;
    [SerializeField] private string nextScene = "MenuScene";

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(nextScene);
    }
}