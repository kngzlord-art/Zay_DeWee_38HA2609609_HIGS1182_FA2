using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public float Health = 100;
    public Slider HealthSlider;

    [Header("Other Stats")]
    public int Money = 500;
    public int maxMoney = 500;

    public int Score = 0;

    [Header("Fuel Settings")]
    public float Fuel = 10;
    public int maxFuel = 100;
    public Slider FuelSlider;

    [Header("Displays")]
    public TextMeshProUGUI moneyDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI healthDisplay;

    private void Awake()
    {
        Health = Mathf.Clamp(Health, 0, maxHealth);
        Money = Mathf.Clamp(Money, 0, maxMoney);
        Fuel = Mathf.Clamp(Fuel, 0, maxFuel);
    }

    private void Start()
    {
        RefreshUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        HealthSlider.value = Health;
        FuelSlider.value = Fuel;

        if (FuelSlider != null)
        {
            FuelSlider.value = Fuel;
        }

        UpdateMoneyDisplay();
        UpdateScoreDisplay();
    }

    public void hurt(float amount)
    {
        Health -= amount;
        Health = Mathf.Clamp(Health, 0, maxHealth);

        UpdateUI();
    }

    public void heal(int amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, maxHealth);

        UpdateUI();
    }

    public void losefuel(float amount)
    {
        Fuel -= amount;
        Fuel = Mathf.Clamp(Fuel, 0, maxFuel);

        UpdateUI();
    }

    public void addFuel(int amount)
    {
        Fuel += amount;
        Fuel = Mathf.Clamp(Fuel, 0, maxFuel);

        UpdateUI();
    }

    public void loseMoney(int amount)
    {
        Money -= amount;
        Money = Mathf.Clamp(Money, 0, maxMoney);

        UpdateUI();
    }

    public void addMoney(int amount)
    {
        Money += amount;
        Money = Mathf.Clamp(Money, 0, maxMoney);

        UpdateUI();
    }

    public void addScore(int amount)
    {
        Score += amount;

        UpdateUI();
    }

    private void UpdateMoneyDisplay()
    {
        if (moneyDisplay != null)
        {
            moneyDisplay.text = "Money: " + Money;
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreDisplay != null)
        {
            scoreDisplay.text = "Score: " + Score;
        }
    }

    public void RefreshUI()
    {
        if (HealthSlider != null)
        {
            HealthSlider.minValue = 0;
            HealthSlider.maxValue = maxHealth;
            HealthSlider.value = Health;
        }

        if (FuelSlider != null)
        {
            FuelSlider.minValue = 0;
            FuelSlider.maxValue = maxFuel;
            FuelSlider.value = Fuel;
        }

        UpdateMoneyDisplay();
        UpdateScoreDisplay();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ForceRefresh();
        }
    }
}