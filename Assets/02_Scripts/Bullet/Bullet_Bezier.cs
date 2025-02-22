using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Bezier : MonoBehaviour
{
    [Header("## -- Bullet_Bezier -- ##")]
    public float Bezier_damage;
    public Bullet Bullet;
    public GameObject Fire_Point;

    private void OnEnable()
    {
        gameObject.transform.position = Fire_Point.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) gameObject.SetActive(false);
    }
}
