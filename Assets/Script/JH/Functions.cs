using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    public static void Hit_Knock_Back(GameObject hitObj, GameObject hitedObj, float power = 20f)
    {
        if (hitObj.transform.position.x - hitedObj.transform.position.x > 0)
            hitedObj.GetComponent<Rigidbody2D>().AddForce(Vector3.left * power, ForceMode2D.Impulse);
        else if (hitObj.transform.position.x - hitedObj.transform.position.x < 0)
            hitedObj.GetComponent<Rigidbody2D>().AddForce(Vector3.right * power, ForceMode2D.Impulse);
    }

    public static IEnumerator Blink_Color(GameObject obj, Color color, float n = 3)
    {
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        Color origin = sprite.color;
        for (int i = 0; i < n; i++)
        {
            sprite.color = color;
            yield return new WaitForSeconds(0.2f);
            sprite.color = origin;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
