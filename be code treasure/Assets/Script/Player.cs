using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour

{
    public CharacterDatabase characterDB;
    public SpriteRenderer atWorkSprite;
    private int SelectedOption = 0; // to keep track which char being selected
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
      private void UpdateCharacter(int SelectedOption) // update the char sprite and name
	{
        Character character= characterDB.GetCharacter(SelectedOption);
        atWorkSprite.sprite = character.characterSprite;

	}

    private void Load()
	{
       SelectedOption = PlayerPrefs.GetInt("SelectedOption");
	}


}
