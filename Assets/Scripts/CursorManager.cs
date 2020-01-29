using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    #region variables
    TcpServer tcp = new TcpServer();
    Tracker tracker = new Tracker();
    float zCache = 3;
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
                // zCache = (float)Math.Round(tracker.position.z, 1);
                zCache = 3;
                zCacheSet = true;
            }

            tracker = JsonUtility.FromJson<Tracker>(tcp.stream.Dequeue());

            if (Math.Abs(tracker.position.x) > 0.4)
                transform.Translate(Time.deltaTime * 3 * (float)Math.Round(tracker.position.x, 1), 0, 0, Space.Self);

            if (Math.Abs(tracker.position.y) > 0.4)
                transform.Translate(0, Time.deltaTime * 3 * (float)Math.Round(tracker.position.y, 1), 0, Space.Self);

            if (Math.Abs(tracker.position.z - zCache) > 0.4)
                transform.Translate(0, 0, Time.deltaTime * 3 * ((float)Math.Round(tracker.position.z, 1) - zCache), Space.Self);

            if (Math.Abs(tracker.position.x) > 0.4 && Math.Abs(tracker.position.y) > 0.4 && Math.Abs(tracker.position.z) > 0.4)
                transform.GetComponent<Renderer>().material.color = Color.white;
            else if (tracker.position.z - zCache > 0.4) transform.GetComponent<Renderer>().material.color = Color.red;
            else if (tracker.position.z - zCache < -0.4) transform.GetComponent<Renderer>().material.color = Color.blue;
            else transform.GetComponent<Renderer>().material.color = Color.yellow;

            // transform.Rotate(
            //     Time.deltaTime * 3 * (float)Math.Round(tracker.rotation.x, 1),
            //     Time.deltaTime * 3 * (float)Math.Round(tracker.rotation.y, 1),
            //     Time.deltaTime * 3 * (float)Math.Round(tracker.rotation.z, 1),
            //     Space.Self
            // );
        }
    }

    void Destroy() => tcp.ServerStop();

    void OnGUI()
    {
        if (Math.Abs(tracker.position.x) > 0.4) GUI.Label(new Rect(100, 100, 100, 100), "X moving " + Math.Round(tracker.position.x, 1));
        if (Math.Abs(tracker.position.y) > 0.4) GUI.Label(new Rect(100, 150, 100, 100), "Y moving " + Math.Round(tracker.position.y, 1));
        if (Math.Abs(tracker.position.z - zCache) > 0.4) GUI.Label(new Rect(100, 200, 100, 100), "Z moving " + Math.Round(tracker.position.z - zCache, 1));
    }
}