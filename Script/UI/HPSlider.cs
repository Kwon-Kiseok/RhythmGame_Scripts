using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPSlider : MonoBehaviour
{
    private PlayerController player;
    private Slider slider;
    public TextMeshProUGUI hpText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        slider.value = (float)player.CurrentHP / player.MaxHP;

        hpText.text = String.Format("{0:D3}/{1:D3}", player.CurrentHP, player.MaxHP);
    }
}
