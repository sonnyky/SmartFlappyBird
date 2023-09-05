using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSet : MonoBehaviour
{
    [SerializeField] MeshRenderer seabed;
    [SerializeField] MeshRenderer ceiling;

    // Based on the bottom obstacle
    [SerializeField] SpriteRenderer obstacle;

    [SerializeField] SpriteRenderer player;
    float mPlayerWidth;

    float mObstacleWidth;
    float mObstacleHeight;
    float mHorizontalDistanceBetweenObstacles = 5f;
    float mDistanceOfNextObstacle = 4f;

    // Calculated height and distance between player and obstacles
    float mHorizontalDistancePlayerObstacle;
    float mVerticalDistancePlayerObstacle;

    // Start is called before the first frame update
    void Start()
    {
        mObstacleWidth = obstacle.bounds.size.x;
        mObstacleHeight = obstacle.bounds.size.y;

        mPlayerWidth = player.bounds.size.x;

        mHorizontalDistancePlayerObstacle = mHorizontalDistanceBetweenObstacles;
        mVerticalDistancePlayerObstacle = seabed.transform.localPosition.y + obstacle.transform.localPosition.y + (mObstacleHeight/2);

        Debug.Log("player width: " + mPlayerWidth + " and obstacle width and height: "  +  mObstacleWidth + ", " + mObstacleHeight );
    }

    public void InitiatePosition()
    {
        int i = 0;
        foreach (Transform t in transform)
        {
            if (t.gameObject.tag == "Obstacle")
            {
                t.localPosition = new Vector3(i * mHorizontalDistanceBetweenObstacles, 0f, 0f);
            }
            i++;
        }
    }

    public float GetVerticalDistanceToNextObstacle()
    {
        return mVerticalDistancePlayerObstacle;
    }

    public float GetHorizontalDistanceToNextObstacle()
    {
        return mHorizontalDistancePlayerObstacle;
    }
    public Transform GetNextObstacle()
    {
        Transform leftChild = transform.Find("ObstacleParent0");
        //Debug.Log("position of first pipe : " + leftChild.position);
        foreach (Transform t in transform)
        {
            float distance =  t.localPosition.x;
            if (t.gameObject.tag == "Obstacle" && distance < mDistanceOfNextObstacle + mObstacleWidth + mPlayerWidth && distance > 0f)
            {
                leftChild = t;
                mHorizontalDistancePlayerObstacle = distance;
                mVerticalDistancePlayerObstacle = leftChild.localPosition.y + (mObstacleHeight / 2);

                break;
            }
        }
        return leftChild;
    }
}
