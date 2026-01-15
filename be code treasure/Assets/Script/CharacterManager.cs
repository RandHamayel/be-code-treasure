using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour

{
    public CharacterDatabase characterDB;
	public Image characterImage;
    public Text characterName;
    public Button nextButton;
    public Button backButton;
    public Button playButton;

	

    private AvatarData[] avatars;
    private int currentIndex = 0;
    public Text nameText;
    public SpriteRenderer atWorkSprite;
    private int SelectedOption = 0; // to keep track which char being selected

    void Start()
    {
		if (!PlayerPrefs.HasKey("SelectedOption"))
		{
			SelectedOption = 0;
		}
		else
		{
			Load();
		}
        UpdateCharacter(SelectedOption);
    }
    public void NextOPtion() // when player click next Button
	{
        SelectedOption++;
        if (SelectedOption >= characterDB.CharacterCount)
		{
            SelectedOption = 0;
		}

        UpdateCharacter(SelectedOption);
        Save();

	}
    public void BackOption()
	{
		SelectedOption--;

        if (SelectedOption < 0)
		{
			SelectedOption = characterDB.CharacterCount - 1;

		}

        UpdateCharacter(SelectedOption);
        Save();

	}
    private void UpdateCharacter(int SelectedOption) // update the char sprite and name
	{
        Character character= characterDB.GetCharacter(SelectedOption);
        atWorkSprite.sprite = character.characterSprite;
        nameText.text = character.charactersName;
	}

    private void Load()
	{
       SelectedOption = PlayerPrefs.GetInt("SelectedOption");
	}
    private void Save()
	{
        PlayerPrefs.SetInt("SelectedOption", SelectedOption);
	}
    public void ChangeScene(int sceneID)
	{
		SceneManager.LoadScene(4);
	}
}
