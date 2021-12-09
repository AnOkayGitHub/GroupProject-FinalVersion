using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardMouse : MonoBehaviour
{
    [SerializeField] private float offset;
    private PlayerController pc;

    private void Start()
    {
        StartCoroutine("LateStart");
    }

    // Update is called once per frame
    void Update()
    {
        if(pc == null)
        {
            pc = World.player.GetComponent<PlayerController>();
        }
        else
        {
            if (!pc.GetIsBusy() && !pc.GetIsPaused())
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 dir = Input.mousePosition - pos;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            }
        }
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        pc = World.player.GetComponent<PlayerController>();
    }
}
