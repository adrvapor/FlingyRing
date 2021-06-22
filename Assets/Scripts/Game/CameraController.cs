using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform bg1;
    public Transform bg2;

    private float bgSize;
    private float highestY;

    // Start is called before the first frame update
    void Start()
    {
        bgSize = bg1.GetComponent<BoxCollider2D>().size.y * 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        float playerPosY = player ? player.position.y : 0;

        if (playerPosY >= highestY) highestY = playerPosY;
        if (playerPosY >= bg2.position.y)
        {
            bg1.position = new Vector3(bg1.position.x, bg1.position.y + bgSize, bg1.position.z);
            SwitchBg();
        }

        Vector3 targetPos = new Vector3(transform.position.x, highestY);

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
    }

    private void SwitchBg()
    {
        Transform aux = bg1;
        bg1 = bg2;
        bg2 = aux;
    }
}
