/*
© Dyno Robotics, 2019
Author: Samuel Lindgren (samuel@dynorobotics.se)
Licensed under the Apache License, Version 2.0
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rclcs;

public class UnityInputTeleop : MonoBehaviourRosNode
{
    public string NodeName = "unity_teleop";
    public string CommandVelocityTopic = "cmd_vel";

    public float MaxForwardVelocity = 1.0f;
    public float MaxSidewaysVelocity = 1.0f;
    public float MaxRotationalVelocity = 3.0f;

    public float PublishingFrequency = 20.0f;

    public bool UseHolonomicControls = false;
    protected override string nodeName { get { return NodeName; } }

    private Publisher<geometry_msgs.msg.Twist> cmdVelPublisher;
    private geometry_msgs.msg.Twist cmdVelMsg;

    private bool enableUserControl;

    protected override void StartRos()
    {
        cmdVelPublisher = node.CreatePublisher<geometry_msgs.msg.Twist>(CommandVelocityTopic);
        cmdVelMsg = new geometry_msgs.msg.Twist();
        enableUserControl = true;
        StartCoroutine("PublishCommandVelocity");
    }

    IEnumerator PublishCommandVelocity()
    {
        for (;;)
        {
            cmdVelPublisher.Publish(cmdVelMsg);
            yield return new WaitForSeconds(1.0f / PublishingFrequency);
        }
    }

    public float GetAngularVelocity()
    {
        return (float) cmdVelMsg.Angular.Z;
    }

    public float GetLinearVelocity()
    {
        return (float) cmdVelMsg.Linear.X;
    }

    public void EnableUserControl(bool enable)
    {
        enableUserControl = enable;
    }

    public void MoveAngular(float k)
    {
        cmdVelMsg.Angular.Z = -k * MaxRotationalVelocity;
    }

    public void MoveLinear(float k)
    {
        cmdVelMsg.Linear.X = k * MaxForwardVelocity;
    }

    private void Update()
    {
        if (enableUserControl)
        {
            cmdVelMsg.Linear.X = Input.GetAxis("Vertical") * MaxForwardVelocity;
            if (UseHolonomicControls)
            {
                cmdVelMsg.Linear.Y = -Input.GetAxis("Horizontal") * MaxSidewaysVelocity;
                cmdVelMsg.Angular.Z = -Input.GetAxis("Turning") * MaxRotationalVelocity;
            }
            else
            {
                cmdVelMsg.Angular.Z = -Input.GetAxis("Horizontal") * MaxRotationalVelocity;
            }
        }
    }
}
