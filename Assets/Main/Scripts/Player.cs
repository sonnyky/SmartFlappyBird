using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    // Flap force
    public float m_Force = 300;

    private float timer = 0.0f;
    private float m_TimeStepToAddScore = 3f;
    public delegate void Survived();
    public event Survived OnPlayerSurvived;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * m_Force);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (transform.position.y > 6f || transform.position.y < -6f)
        {
            SceneManager.LoadScene(0);
        }
        timer += Time.deltaTime;
        if (timer > m_TimeStepToAddScore)
        {
            timer = timer - m_TimeStepToAddScore;
            if (OnPlayerSurvived != null) OnPlayerSurvived.Invoke();
        }
    }
}
