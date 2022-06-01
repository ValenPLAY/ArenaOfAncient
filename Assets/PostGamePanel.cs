using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostGamePanel : MonoBehaviour
{
    [SerializeField] TMP_Text postGameText;
    private void OnEnable()
    {
        postGameText.text = "";
        postGameText.text += "Waves survived: " + LoadingController.Instance.wavesSurvived;
        postGameText.text += "<br> Enemies killed: " + LoadingController.Instance.enemiesKilled;
        postGameText.text += "<br> Total Essense Earned: " + 0;
    }
}
