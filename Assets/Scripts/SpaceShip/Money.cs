using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private int coinsCounter;

    public void AddCoins(int count)
    {
        coinsCounter += count;
        PlayerPrefs.SetInt("PlayerMoney",coinsCounter);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("PlayerMoney"));
    }
}
