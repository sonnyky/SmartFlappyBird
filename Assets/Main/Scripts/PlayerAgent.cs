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

    public float counter;

    float mFlap = 0f;

    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        counter += Time.deltaTime;
    }

    public override void OnEpisodeBegin()
    {
        mRigidBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
        mObstacleSet.InitiatePosition();
        mGameOver = false;
        counter = 0;
        mScreenPressed = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Next Pipe position
        Transform nextPipe = mObstacleSet.GetNextObstacle();

        // Agent positions
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
        actionsOut.DiscreteActions.Array[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void Push()
    {
        mRigidBody.AddForce(Vector2.up * 4f, ForceMode2D.Impulse);
    }

    public override void OnActionReceived(ActionBuffers vectorAction)
    {
        AddReward(Time.fixedDeltaTime);
        int tap = vectorAction.DiscreteActions[0];
        if (tap == 0)
        {
            mScreenPressed = false;
        }
        if (tap == 1 && !mScreenPressed)
        {
            Push();
            mScreenPressed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision2d)
    {
        SetReward(-1f);
        EndEpisode();
    }

    public float Normalize(float max, float min, float value)
    {
        //current - min
        return (value - min) / (max - min);
    }

}
