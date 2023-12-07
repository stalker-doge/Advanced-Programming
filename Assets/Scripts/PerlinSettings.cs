using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerlinSettings : MonoBehaviour
{

    [SerializeField] Slider scaleSlider;
    [SerializeField] Slider cellSlider;
    [SerializeField] Slider widthSlider;
    [SerializeField] Slider lacunaritySlider;
    [SerializeField] Slider persistanceSlider;
    [SerializeField] Slider octavesSlider;
    [SerializeField] Slider seedSlider;
    [SerializeField] TMP_Text scaletext;
    [SerializeField] TMP_Text celltext;
    [SerializeField] TMP_Text widthtext;
    [SerializeField] TMP_Text lacunarityText;
    [SerializeField] TMP_Text persistanceText;
    [SerializeField] TMP_Text octavesText;
    [SerializeField] TMP_Text seedText;
    [SerializeField] GameObject testPlane;
    [SerializeField] GameObject Plane;
    // Start is called before the first frame update
    void Start()
    {
        //Perlin.Instance.Generate((int)widthSlider.value, (int)widthSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        scaletext.text = scaleSlider.value.ToString();
        widthtext.text = widthSlider.value.ToString();
        celltext.text = cellSlider.value.ToString();
        lacunarityText.text = lacunaritySlider.value.ToString();
        persistanceText.text = persistanceSlider.value.ToString();
        octavesText.text = octavesSlider.value.ToString();
        seedText.text = seedSlider.value.ToString();
        MeshGeneration.Instance.SetHeight((int)widthSlider.value);
        MeshGeneration.Instance.SetWidth((int)widthSlider.value);
        MeshGeneration.Instance.SetCellSize((int)cellSlider.value);
        Perlin.Instance.scale = scaleSlider.value;
        Perlin.Instance.lacunarity = lacunaritySlider.value;
        Perlin.Instance.persistance = persistanceSlider.value;
        Perlin.Instance.octaves = (int)octavesSlider.value;
        Perlin.Instance.seed= (int)seedSlider.value;
        Perlin.Instance.Generate((int)widthSlider.value, (int)widthSlider.value);
        MeshGeneration.Instance.GenerateMesh();
    }

    public void Apply()
    {
        MeshGeneration.Instance.SetHeight((int)widthSlider.value);
        MeshGeneration.Instance.SetWidth((int)widthSlider.value);
        MeshGeneration.Instance.SetCellSize((int)cellSlider.value);
        FindObjectOfType<Canvas>().gameObject.SetActive(false);
        Perlin.Instance.scale = scaleSlider.value;
        Perlin.Instance.lacunarity = lacunaritySlider.value;
        Perlin.Instance.persistance = persistanceSlider.value;
        Perlin.Instance.octaves = (int)octavesSlider.value;
        Perlin.Instance.seed = (int)seedSlider.value;
        Perlin.Instance.Generate((int)widthSlider.value, (int)widthSlider.value);
        MeshGeneration.Instance.GenerateMesh();
    }

    
    
}
