using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BounceControllerUI : MonoBehaviour
{
    [Header("Ball")]
    [SerializeField] BounceController bounceController;

    [Header("Score")]
    [SerializeField] TMPro.TextMeshProUGUI text;

    [Header("Buttons")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;

    void Start()
    {
        saveButton.onClick.AddListener(bounceController.SaveBounces);

        //al cargar se llama a LoadBounces y luego se actualiza la UI
        loadButton.onClick.AddListener(() => //funcion anonima
        {
            bounceController.LoadBounces();
            UpdateUI();
        });

        UpdateUI();
    }

    private void OnEnable()
    {
        bounceController.onBouncedOffGround.AddListener(UpdateUI);
    }
    private void OnDisable()
    {
        bounceController.onBouncedOffGround.RemoveListener(UpdateUI);
    }

    void UpdateUI()
    {
        text.text = bounceController.GetBounces().ToString();
    }

}
