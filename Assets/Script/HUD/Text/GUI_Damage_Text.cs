using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Damage_Text : MonoBehaviour
{
    public float move_speed;
    public float alpha_speed;
    public TextMeshPro hit_damage_text;
    public float destroy_time;
    Color alpha;
    public float player_damage;

    public Transform target; // 따라갈 몬스터의 Transform
    public Camera mainCamera;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        hit_damage_text = GetComponent<TextMeshPro>();
        hit_damage_text.text = $"{player_damage}";
        alpha = hit_damage_text.color;
        Invoke("DestroyObject", destroy_time);
        // HeadMarker 위치로 이동
        transform.position = target.position;
        // UI가 카메라를 항상 바라보도록 설정
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, move_speed * Time.deltaTime,0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alpha_speed);
        hit_damage_text.color = alpha;
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetDamage(Transform followTarget)
    {
        target = followTarget;
    }
}
