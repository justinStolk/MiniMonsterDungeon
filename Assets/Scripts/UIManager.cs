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

    [SerializeField] private Button waitButton;
    [SerializeField] private Button attackButton;

    [SerializeField] private GameObject dungeonMenu;

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
        dungeonMenu.SetActive(false);
        EventSystem.Subscribe(EventType.ON_PLAYER_MOVED, () => waitButton.gameObject.SetActive(true));
        EventSystem.Subscribe(EventType.ON_DUNGEON_MENU_TOGGLED, () => dungeonMenu.SetActive(!dungeonMenu.activeSelf));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfirmPlayerMovement()
    {
        waitButton.gameObject.SetActive(false);
        EventSystem.CallEvent(EventType.ON_PLAYER_MOVE_CONFIRMED);
    }

    public void UpdateBattleForecast(int expectedDamage, int healthToDisplay)
    {
        forecastDamage.text = expectedDamage.ToString();
        forecastHealth.text = healthToDisplay.ToString();
        forecastInterface.SetActive(true);
    }

}
