using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [HideInInspector] public bool IsAnimal;
    [HideInInspector] public int X, Y;
    
    [SerializeField] private Text TextField;
    [SerializeField] private Image AnimalImage;
    
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(FoundAnimal);
    }

    private void FoundAnimal()
    {
        if (IsAnimal)
        {
            AudioManager.Instance.PlaySound("AnimalFound");
            AnimalImage.gameObject.SetActive(true);
            GameController.Instance.AnimalsFound();
        }
        else
        {
            AudioManager.Instance.PlaySound("CellSound");
            GameController.Instance.CountPoints();
            TextField.text = GameController.Instance.FindAnimals(X, Y).ToString();
        }

        button.enabled = false;
    }
}
