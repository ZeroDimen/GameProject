using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float hp;
    protected IEnumerator Blink(GameObject obj, int n = 4)
    {
        for (int i = 0; i < n; i++)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.5f);
            obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected IEnumerator Blink_Color(GameObject obj, Color color, int n = 4)
    {
        Color originColor = obj.GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < n; i++)
        {
            obj.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.5f);
            obj.GetComponent<SpriteRenderer>().color = originColor;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
