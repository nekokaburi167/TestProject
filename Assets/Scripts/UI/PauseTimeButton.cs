using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseTimeButton : MonoBehaviour
{
    private GameManager m_gameManager;
    public Image image;
    public Sprite[] sprite;
     
    private void Start()
    {
        m_gameManager = GameManager.Instance;
        
    }
    public void PauseClicked()
    {
        if(m_gameManager.m_gameState == GameState.Frozen)
        {
            m_gameManager.SetGameState(GameState.Game, null);
            image.sprite = sprite[0];
            
        }
        else
        {
            m_gameManager.SetGameState(GameState.Frozen, null);
            image.sprite = sprite[1];
        }
    }

}
