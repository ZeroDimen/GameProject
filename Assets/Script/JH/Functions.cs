using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    public static void Hit_Knock_Back(GameObject hitObj, GameObject hitedObj, float power = 20f)
    {
        Rigidbody2D rigid = hitedObj.GetComponent<Rigidbody2D>();

        if (rigid.bodyType == RigidbodyType2D.Kinematic)
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.gravityScale = 0f;
        }

        if (hitObj.transform.position.x - hitedObj.transform.position.x > 0)
            hitedObj.GetComponent<Rigidbody2D>().AddForce(Vector3.left * power, ForceMode2D.Impulse);
        else if (hitObj.transform.position.x - hitedObj.transform.position.x < 0)
            hitedObj.GetComponent<Rigidbody2D>().AddForce(Vector3.right * power, ForceMode2D.Impulse);
    }
}
