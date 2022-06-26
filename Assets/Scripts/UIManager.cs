using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject forecastInterface;
    [SerializeField] private TextMeshProUGUI forecastDamage;
    [SerializeField] private TextMeshProUGUI forecastHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this);

        forecastInterface.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBattleForecast(int expectedDamage, int healthToDisplay)
    {
        forecastDamage.text = expectedDamage.ToString();
        forecastHealth.text = healthToDisplay.ToString();
        forecastInterface.SetActive(true);
    }

}
