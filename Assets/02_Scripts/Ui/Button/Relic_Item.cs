using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class Relic_Item : MonoBehaviour
{
    public List<Relic_Data> Relic = new List<Relic_Data>();
    public Relic_Data data;
    public Relic_Data relic_id_num;
    public int level;
    public Image relic_icon;

    public Button[] Select_Button;

    string filePath;
    string png_name;
    Text text_name;
    Text text_desc;


    private void OnEnable()
    {
        List<Relic_Data> randomRelics = GetRandomRelics(3); // 3개 선택
        List<Relic_Data> removedRelics = new List<Relic_Data>(); // 삭제된 데이터를 저장할 리스트
        for (int i = 0;  i < Relic.Count; i++)
        {
            data = GetRandomFromList(randomRelics);
            relic_icon = Select_Button[i].GetComponentsInChildren<Image>()[2];
            Text[] texts = Select_Button[i].GetComponentsInChildren<Text>();
            text_name = texts[0];
            text_desc = texts[1];
            text_name.text = data.Relics_Name;
            level = data.Relics_Lv;
            png_name = $"{data.Relic_lcon}" + ".png";
            filePath = Path.Combine(Application.dataPath, $"07_Textures/Icon/Relics_Icon/{png_name}");
            LoadImage(filePath);
            text_name.text = data.Relics_Name;
            text_desc.text = string.Format(data.item_desc);

            removedRelics.Add(data);
            randomRelics.Remove(data);
        }
        // **삭제된 3개 복구**
        randomRelics.AddRange(removedRelics);
    }
    private void LoadImage(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath); // 파일 데이터를 바이트 배열로 읽음
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData); // Texture2D에 이미지 적용

        // Texture2D를 Sprite로 변환 후 Image에 적용
        relic_icon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    //리스트에서 원하는 개수만큼 랜덤으로 선택 (중복 없음)
    public List<Relic_Data> GetRandomRelics(int count)
    {
        if (Relic.Count == 0)
        {
            return new List<Relic_Data>();
        }

        return Relic.OrderBy(x => Random.value).Take(count).ToList();
    }

    //3개 뽑은 리스트에서 다시 하나를 랜덤으로 선택
    public Relic_Data GetRandomFromList(List<Relic_Data> relics)
    {
        if (relics == null || relics.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, relics.Count);
        return relics[randomIndex];
    }
}
