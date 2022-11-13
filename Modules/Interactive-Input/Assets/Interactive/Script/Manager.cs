using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return Data.Plist.GetConfig();

        camera.Init();
        player.Init();
    }

    public Interactive.Player.PlayerClient player;
    public Interactive.CameraControl.CameraCtr camera;




}
