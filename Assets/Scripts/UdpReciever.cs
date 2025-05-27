using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;
using System;


public class UdpReceiver : MonoBehaviour
{
    [SerializeField] private int port = 14550;
    [SerializeField] private DroneController droneController;

    private UdpClient listener;
    private byte[] receivedBytes;
    private readonly int receiveTimeoutDurationMs = 2000;
    private Thread listenThread;
    private bool running;
    private IPEndPoint remoteIpEndPoint;
    private bool isFirstGpsPacket = true;

    private void Start()
    {
        StartListening();
    }

    private void OnDisable()
    {
        running = false;
        listener.Close();
    }

    private void StartListening()
    {
        remoteIpEndPoint = new IPEndPoint(IPAddress.Any, port);
        listener = new UdpClient(port);

        if (listenThread != null)
        {
            StopThread();
        }

        listenThread = new Thread(StartReceiver);
        listenThread.Start();
    }

    private void StopThread()
    {
        running = false;

        listenThread.Join();
    }

    void StartReceiver()
    {
        listener.Client.ReceiveTimeout = receiveTimeoutDurationMs; // in miliseconds

        running = true;
        while (running)
        {
            GetPacket();
        }

        listener.Close();
    }


    void GetPacket()
    {
        receivedBytes = null;

        try
        {
            receivedBytes = listener.Receive(ref remoteIpEndPoint);

            if (receivedBytes != null)
            {
                MAVLink.MAVLinkMessage mavMessage = new MAVLink.MAVLinkMessage(receivedBytes);

                if (mavMessage.data is MAVLink.mavlink_global_position_int_t gpsPacket)
                {
                    double latitude = gpsPacket.lat / 1e7;
                    double longitude = gpsPacket.lon / 1e7;
                    double altitude = gpsPacket.alt / 1000.0;
                    if (isFirstGpsPacket)
                    {
                        GpsConverter.InitialiseOrigin(latitude, longitude, altitude);
                        isFirstGpsPacket = false;
                    }

                    Vector3 unityPosition = GpsConverter.UnityPositionFromLatLonAlt(latitude, longitude, altitude);
                    droneController.SetPosition(unityPosition);
                }
                else if (mavMessage.data is MAVLink.mavlink_attitude_t attitude)
                {
                    float roll = attitude.roll;
                    float pitch = attitude.pitch;
                    float yaw = attitude.yaw;
                    Quaternion attitudeQuaternion = AttitudeConverter.ConvertMavlinkAttitudeToUnity(roll, pitch, yaw);
                    droneController.SetRotation(attitudeQuaternion);
                }
                else if (mavMessage.data is MAVLink.mavlink_servo_output_raw_t servoOutputRaw)
                {
                    float[] pwms = new float[4];
                    pwms[0] = servoOutputRaw.servo1_raw;
                    pwms[1] = servoOutputRaw.servo2_raw;
                    pwms[2] = servoOutputRaw.servo3_raw;
                    pwms[3] = servoOutputRaw.servo4_raw;
                    droneController.SetPropellersPWM(pwms);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        Thread.Sleep(1);
    }
}