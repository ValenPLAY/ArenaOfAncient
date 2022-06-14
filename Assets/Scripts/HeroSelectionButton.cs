using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectionButton : MonoBehaviour
{
    public Image heroButtonImage;
    public int heroButtonID;
    public Action<int> onButtonHeroClick;

    public void ChangeIcon(Sprite heroImage)
    {
        if (heroImage != null)
        {
            heroButtonImage.sprite = heroImage;
        }
    }

    public void ButtonClick()
    {
        onButtonHeroClick?.Invoke(heroButtonID);
    }
}
