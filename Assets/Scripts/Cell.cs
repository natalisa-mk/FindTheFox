using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [HideInInspector] public bool IsAnimal;
    [HideInInspector] public int X, Y;
    
    [SerializeField] private Text textField;
    [SerializeField] private Image animalImage;
    
    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(FoundAnimal);
    }

    private void FoundAnimal()
    {
        if (IsAnimal)
        {
            AudioManager.Instance.PlaySound("AnimalFound");
            animalImage.gameObject.SetActive(true);
            GameController.Instance.AnimalsFound();
        }
        else
        {
            AudioManager.Instance.PlaySound("CellSound");
            GameController.Instance.CountPoints();
            textField.text = GameController.Instance.FindAnimals(X, Y).ToString();
        }

        _button.enabled = false;
    }
}
