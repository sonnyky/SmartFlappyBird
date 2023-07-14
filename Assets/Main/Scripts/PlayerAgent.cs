using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent
{
    Rigidbody2D mRigidBody;

    [SerializeField]
    ObstacleSet mObstacleSet;

    [SerializeField]
    GameObject seabed;

    bool mGameOver = false;

    bool mScreenPressed = false;

    float mFlap = 0f;

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
        // Next Pipe position
        Transform nextPipe = mObstacleSet.GetNextObstacle();

        // Agent positions normalized to [0, 1]
        float playerHeightFromSeabed = transform.localPosition.y - seabed.transform.position.y;
        sensor.AddObservation(playerHeightFromSeabed);

        float hDistanceToNextObstacle = mObstacleSet.GetHorizontalDistanceToNextObstacle();
        sensor.AddObservation(hDistanceToNextObstacle);

        float vDistanceToNextObstacle = mObstacleSet.GetVerticalDistanceToNextObstacle();
        sensor.AddObservation(vDistanceToNextObstacle);

        // Agent velocity
        sensor.AddObservation(mRigidBody.velocity.y);

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
