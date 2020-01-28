using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    #region variables
    TcpServer tcp = new TcpServer();
    TrackerState tracker = new TrackerState();
    float zCache = 0;
    bool zCacheSet = false;
    #endregion

    public bool isConnected() => tcp.isConnected();
    public void ServerStart(String IP, int port) => tcp.ServerStart(IP, port);
    public void ServerStop() => tcp.ServerStop();

    void Update()
    {
        if (tcp.isConnected() && tcp.stream.Count != 0)
        {
            if (!zCacheSet)
            {
                zCache = (float)Math.Round(tracker.position.z, 1);
                zCacheSet = true;
            }

            tracker = JsonUtility.FromJson<TrackerState>(tcp.stream.Dequeue());

            transform.Translate(
                Time.deltaTime * 100 * (float)Math.Round(tracker.position.x, 1),
                Time.deltaTime * 100 * (float)Math.Round(tracker.position.y, 1),
                Time.deltaTime * 100 * ((float)Math.Round(tracker.position.z, 1) - zCache),
                Space.Self
            );

            if (tracker.position.z - zCache > 0.1) transform.GetComponent<Renderer>().material.color = Color.red;
            else if (tracker.position.z - zCache < 0.1) transform.GetComponent<Renderer>().material.color = Color.blue;
            else transform.GetComponent<Renderer>().material.color = Color.white;

            // transform.Rotate(
            //     Time.deltaTime * 10 * (float)Math.Round(tracker.rotation.x, 2),
            //     Time.deltaTime * 10 * (float)Math.Round(tracker.rotation.y, 2),
            //     Time.deltaTime * 10 * (float)Math.Round(tracker.rotation.z, 2),
            //     Space.Self
            // );
        }
    }

    void Destroy() => tcp.ServerStop();
}
