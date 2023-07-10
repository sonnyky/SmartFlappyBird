using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent
{
    Rigidbody2D mRigidBody;
    float mHeight = 4f;

    [SerializeField]
    ObstacleSet mObstacleSet;

    [SerializeField]
    float mForceMultiplier = 300f;

    bool mGameOver = false;

    bool mScreenPressed = false;

    float mFlap = 0f;

    string currentObstacleName = "";

    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            mFlap = 1f;
        }
        else
        {
            mFlap = 0f;
        }


        if (transform.position.y > 6f || transform.position.y < -6f)
        {
            mGameOver = true;
            Debug.Log("outside the bounds");
        }
    }

    public override void OnEpisodeBegin()
    {
        mRigidBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
        mObstacleSet.InitiatePosition();
        mGameOver = false;
        mScreenPressed = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent positions normalized to [0, 1]
        float playerHeightNormalized = (transform.localPosition.y + mHeight) / (mHeight * 2);
        //Debug.Log("playerHeightNormalized: " + playerHeightNormalized);
        sensor.AddObservation(playerHeightNormalized);
        //Debug.Log("Agent position: " + transform.position);
        //Debug.Log("Agent localposition: " + transform.localPosition);

        // Next Pipe position
        Transform nextPipe = mObstacleSet.GetNextObstacle();
        currentObstacleName = nextPipe.gameObject.name;

        //Distance to next obstacle normalized to [0, 1]
        //Debug.Log("Next obstacle position : " + nextPipe.position);
        //Debug.Log("Next obstacle localposition : " + nextPipe.localPosition);
        float s3 = Normalize(5f, 0f, (nextPipe.localPosition.x - transform.localPosition.x + 5f));
        //Debug.Log("distance to next obstacle normalized: " + s3);

        sensor.AddObservation(s3);
        //Debug.Log("next obs is: " + nextPipe.gameObject.name);

        float height = mObstacleSet.GetObstacleHeight();
        float bottomObsHeight = (mHeight + nextPipe.localPosition.y - (6f - (height / 2))) / (mHeight * 2);
        float topObsHeight = (mHeight + nextPipe.localPosition.y + (6f - (height / 2)))/ (mHeight * 2);

        //Debug.Log("nextPipe.localPosition.y  : " + nextPipe.localPosition.y + " (6f - (height / 2) + mHeight: " + (6f - (height / 2) + mHeight));
        //Debug.Log("topObsHeight: " + topObsHeight);
        //Debug.Log("pipe set y: " + nextPipe.localPosition.y + " and pipe height: " + height);

        bottomObsHeight = bottomObsHeight > 0 ? bottomObsHeight : 0f;
        topObsHeight = topObsHeight < mHeight * 2 ? topObsHeight : mHeight * 2;

        // normalized to[0, 1]
        //Debug.Log("bottomObsHeight : " + bottomObsHeight);
        //Debug.Log("topObsHeight: " + topObsHeight);
        sensor.AddObservation(bottomObsHeight);
        sensor.AddObservation(topObsHeight);

        // Agent velocity
        //sensor.AddObservation(mRigidBody.velocity.y);
        sensor.AddObservation(Mathf.Clamp(mRigidBody.velocity.y, -mHeight, mHeight) / mHeight);
        //Debug.Log("mRigidBody.velocity.y: " + mRigidBody.velocity.y);

        // last action
        sensor.AddObservation(mScreenPressed ? 1f : 0f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        actionsOut.DiscreteActions.Array[0] = (int) mFlap;
    }

    private void Push()
    {
        mRigidBody.AddForce(Vector2.up * 4f, ForceMode2D.Impulse);
    }

    public override void OnActionReceived(ActionBuffers vectorAction)
    {
        //Debug.Log("From the network: " + vectorAction[0]);
        if (mGameOver)
        {
            SetReward(-1f);
            EndEpisode();
        }
        else
        {
            
            SetReward(0.01f);
            //Debug.Log("discrete actions: " + vectorAction.DiscreteActions.Length);

            int tap = vectorAction.DiscreteActions[0];
            if (tap == 0f || mScreenPressed)
            {
                mScreenPressed = false;
            }else if(tap == 1f && !mScreenPressed)
            {
                mScreenPressed = true;
                Push();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision2d)
    {
        //Debug.Log("collided with : " + collision2d.gameObject.name);
        mGameOver = true;
    }

    public float Normalize(float max, float min, float value)
    {
        //current - min
        return (value - min) / (max - min);
    }

}
