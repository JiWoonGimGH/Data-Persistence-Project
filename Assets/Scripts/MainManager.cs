using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text Text_playerNameCurrent;
    public Text ScoreText;
    public Text Text_playerNameBestEver;
    public Text Text_ScoreBestEver;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    [SerializeField] int m_Points;

    [SerializeField] bool m_GameOver = false;
    [SerializeField] bool m_YouWin = false;

    [SerializeField] GameObject instructionSpaceBar;
    [SerializeField] GameObject instructionYouWin;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.LoadBest();
        Text_playerNameCurrent.text = GameManager.Instance.playerNameCurrent;

        if (GameManager.Instance.playerNameBestEver == null)
            Text_playerNameBestEver.text = string.Empty;
        else if (GameManager.Instance.playerNameBestEver != null)
            Text_playerNameBestEver.text = GameManager.Instance.playerNameBestEver;
        
        if(GameManager.Instance.ScoreBestEver != 0)
            Text_ScoreBestEver.text = GameManager.Instance.ScoreBestEver.ToString();

        StartCoroutine(StartSpaceBarRoutine());

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    IEnumerator StartSpaceBarRoutine()
    {
        instructionSpaceBar.SetActive(true);

        yield return new WaitForSeconds(4f);

        instructionSpaceBar.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        if (m_Points >= 96)
        {
            m_YouWin = true;
            m_GameOver = true;
            instructionYouWin.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
                return;
        }

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else if(Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(0);
            
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{m_Points}" +" : 96";
    }

    public void GameOver()
    {
        if (m_YouWin)
            return;

        m_GameOver = true;
        GameOverText.SetActive(true);


        if(m_Points > GameManager.Instance.ScoreBestEver)
        {
            GameManager.Instance.ScoreBestEver = m_Points;
            GameManager.Instance.SaveBest();
        }
    }
}
