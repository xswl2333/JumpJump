using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text source;
    public Text bestSource;
    public Text gameOver;
    public Slider power;

    private int m_Source = 0;
    private int m_bestSource = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        gameOver.enabled= false;
        m_Source = 0;
        m_bestSource = PlayerPrefs.GetInt("bestSource");
        RefershSource();
    }

    void RefershSource()
    {
        string str = string.Format("当前成绩:{0}", m_Source);
        source.text = str;
        str = string.Format("最好成绩:{0}", m_bestSource);
        bestSource.text = str;
    }

    public void Addsource(int scr=1)
    {
        m_Source+= scr;
        if (m_Source > m_bestSource)
        {
            m_bestSource=m_Source;
            PlayerPrefs.SetInt("bestSource", m_bestSource);
        }
        RefershSource();
    }
    
    public void SetGameVoer(bool ret=true)
    {
        gameOver.enabled= ret;
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void ShowPower(float value, float maxPower)
    {

        power.minValue = 0;
        power.maxValue = maxPower;
        power.value = value;

    }

}
