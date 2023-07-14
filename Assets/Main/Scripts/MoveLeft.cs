using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 2.5f;

    [SerializeField]
    private bool _randomizeHeight = true;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * m_Speed);
        if (transform.localPosition.x < -5f) // .9f was precalculated. refactor this to increase flexibility!
        {
            if (_randomizeHeight)
            {
                float randomYPosition = UnityEngine.Random.Range(-3, 3);
                transform.localPosition = new Vector3(20f, randomYPosition, 0);
            }
            else
            {
                transform.localPosition = new Vector3(20f, transform.position.y, 0);
            }
        }
    }
}
