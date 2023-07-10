using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSet : MonoBehaviour
{
    float mPlayerWidth;

    float mObstacleWidth;
    float mObstacleHeight;
    float mDistanceOfNextObstacle = 3f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.tag == "Obstacle")
            {
                mObstacleWidth = t.Find("bottom").GetComponent<SpriteRenderer>().bounds.size.x;
                mObstacleHeight = t.Find("bottom").GetComponent<SpriteRenderer>().bounds.size.y;
            }
        }
        mPlayerWidth = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log("player width: " + mPlayerWidth + " and obstacle width and height: "  +  mObstacleWidth + ", " + mObstacleHeight );
    }

    public void InitiatePosition()
    {
        //Debug.Log("Obstacle set initiated");
        float step = 5f;
        int i = 0;
        foreach (Transform t in transform)
        {
            t.localPosition = new Vector3(i * step, 0f, 0f);
            i++;
        }
    }

    public float GetObstacleHeight()
    {
        return mObstacleHeight;
    }

    public float GetObstacleWidth()
    {
        return mObstacleWidth;
    }
    public Transform GetNextObstacle()
    {
        Transform leftChild = transform.Find("ObstacleParent0");
        //Debug.Log("position of first pipe : " + leftChild.position);
        foreach (Transform t in transform)
        {
            float distance = 5f + t.localPosition.x;
            if (t.gameObject.tag == "Obstacle" && distance < mDistanceOfNextObstacle + mObstacleWidth + mPlayerWidth && distance + mObstacleWidth + mPlayerWidth > 0f)
            {
                //Debug.Log("Found one: " + distance);
                //Debug.Log("conditions: " + mDistanceOfNextObstacle + mObstacleWidth + mPlayerWidth + " and : " + distance + mObstacleWidth + mPlayerWidth);
                leftChild = t;
            }
        }
        //Debug.Log("position of next pipe : " + leftChild.position);

        return leftChild;
    }
}
