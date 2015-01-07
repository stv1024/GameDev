using UnityEngine;
using System;

public class PingTest : MonoBehaviour
{
    private void Start()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        Debug.Log(path);
    }

    private Ping _ping = null;

    private void CheckPulse()
    {
    }

    private void zUpdate()
    {
        if (_ping == null && Input.GetKeyUp(KeyCode.A))
        {
            _ping = new Ping("61.155.6.76");
            Debug.Log("new ping @" + Time.realtimeSinceStartup);
        }
        if (_ping != null && _ping.isDone)
        {
            Debug.Log("ping:" + _ping.time + " @" + Time.realtimeSinceStartup);
            _ping = null;
        }
    }
}