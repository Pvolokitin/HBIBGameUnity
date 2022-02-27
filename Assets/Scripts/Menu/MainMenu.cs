using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play(int index)
    {
        SceneManager.LoadScene(index);
    }       //  Кнопка Играть

    public void Exit()
    {
        Application.Quit();
    }       // Кнопка Выход из игры
}
