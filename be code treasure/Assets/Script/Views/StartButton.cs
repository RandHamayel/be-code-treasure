using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    [SerializeField] private string MainMenuPage = "MainMenuScene";
	public void OnStartButtonClicked()
	{
        SceneManager.LoadScene(MainMenuPage);
	}
}
