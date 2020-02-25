using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    #region variables
    TcpServer tcp = new TcpServer();
    Tracker tracker = new Tracker();
    // bool zCacheSet = false;
    float threshold = 0.004f;
    float speedFactor = 20;
    float zCache = 0.5f;
    #endregion

    public bool rotate = false;

    public bool isConnected() => tcp.isConnected();
    public void ServerStart(String IP, int port) => tcp.ServerStart(IP, port);
    public void ServerStop() => tcp.ServerStop();

    void Update()
    {
        if (tcp.isConnected() && tcp.stream.Count != 0)
        {
            // if (!zCacheSet)
            // {
            //     // zCache = (float)Math.Round(tracker.position.z, 1);
            //     zCache = 0.7f;
            //     zCacheSet = true;
            // }

            tracker = JsonUtility.FromJson<Tracker>(tcp.stream.Dequeue());

            if (Math.Abs(tracker.position.x) > threshold)
                transform.Translate(-1 * Time.deltaTime * speedFactor * (float)Math.Round(tracker.position.x, 1), 0, 0, Space.Self);

            if (Math.Abs(tracker.position.y) > threshold)
                transform.Translate(0, Time.deltaTime * speedFactor * (float)Math.Round(tracker.position.y, 1), 0, Space.Self);

            if (Math.Abs(tracker.position.z - zCache) > threshold)
                transform.Translate(0, 0, -1 * Time.deltaTime * speedFactor * ((float)Math.Round(tracker.position.z, 1) - zCache), Space.Self);

            if (Math.Abs(tracker.position.x) > threshold && Math.Abs(tracker.position.y) > threshold && Math.Abs(tracker.position.z) > threshold)
                transform.GetComponent<Renderer>().material.color = Color.white;
            else if (tracker.position.z - zCache > threshold) transform.GetComponent<Renderer>().material.color = Color.red;
            else if (tracker.position.z - zCache < -1 * threshold) transform.GetComponent<Renderer>().material.color = Color.blue;
            else transform.GetComponent<Renderer>().material.color = Color.yellow;

            if (rotate)
            {
                transform.Rotate(
                Time.deltaTime * speedFactor * (float)Math.Round(tracker.rotation.x, 1),
                Time.deltaTime * speedFactor * (float)Math.Round(tracker.rotation.y, 1),
                Time.deltaTime * speedFactor * (float)Math.Round(tracker.rotation.z, 1),
                Space.Self);
            }
        }
    }

    void Destroy() => tcp.ServerStop();

    void OnGUI()
    {
        if (Math.Abs(tracker.position.x) > threshold) GUI.Label(new Rect(100, 100, 100, 100), "X moving " + Math.Round(tracker.position.x, 1));
        if (Math.Abs(tracker.position.y) > threshold) GUI.Label(new Rect(100, 150, 100, 100), "Y moving " + Math.Round(tracker.position.y, 1));
        if (Math.Abs(tracker.position.z - zCache) > threshold) GUI.Label(new Rect(100, 200, 100, 100), "Z moving " + Math.Round(tracker.position.z - zCache, 1));
    }
}