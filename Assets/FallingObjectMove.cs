using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectMove : MonoBehaviour
{
    public static int movespeed = 1;
    public Vector3 userDirection = Vector3.right;
    public FallingObjectCollision FallingObjectCollision;


    public void Update()
    {
        transform.Translate(userDirection * movespeed * Time.deltaTime);
        //StartCoroutine(Stop());
        if (FallingObjectCollision.Dashed)
        {
            userDirection = Vector3.zero;
            movespeed = 0;
        }
    }

    IEnumerable Stop()
    {
        yield return new WaitForSeconds(18f);
    }


}
