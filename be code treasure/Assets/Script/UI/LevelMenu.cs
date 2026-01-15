using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{
	[SerializeField] public UnityEngine.UI.Button[] buttons;
	private void Awake()
	{
		int unlockedlevel = PlayerPrefs.GetInt("UnlockedLevel" , 1);
		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i].interactable = false;
		}
		for(int i = 0; i < unlockedlevel; i++)
		{
			buttons[i].interactable = true;
		}
	}
	public void OpenLevel(int levelId)
	{
       string levelName = "level" + levelId + "Scene";
	   SceneManager.LoadScene(levelName);
	}
}
