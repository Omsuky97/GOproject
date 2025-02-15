using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Boom : MonoBehaviour
{
    public float boom_damage;
    public Bullet Bullet;

    public int segments = 32; // 원형 해상도
    private LineRenderer lineRenderer;
    private SphereCollider sphereCollider;


    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null) return;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        DrawCircle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        boom_damage =  Bullet.damage;
    }

    void DrawCircle()
    {
        float radius = sphereCollider.radius;
        Vector3 center = transform.position + sphereCollider.center;
        Vector3[] points = new Vector3[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            points[i] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) + center;
        }

        lineRenderer.SetPositions(points);
    }
}
