using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TodoObject : MonoBehaviour
{
    [SerializeField] private TMP_Text _Text;
    [SerializeField] private Button _DeleteButton;
    [SerializeField] private Button _EditButton;
    [SerializeField] private Button _DoneButton;
    [SerializeField] private Button _CancelButton;
    [SerializeField] private TMP_InputField _EditIPF;

    private uint _Id;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _EditIPF.gameObject.SetActive(false);
        _DoneButton.gameObject.SetActive(false);
        _CancelButton.gameObject.SetActive(false);
    }

    public void Setup(string text, uint id)
    {
        _Text.text = text;
        _Id = id;
        _DeleteButton.onClick.AddListener(async () =>
        {
            if(await WebRequest.RemoveContent(_Id)) Destroy(gameObject);
        });
        _EditButton.onClick.AddListener(Edit);
        _DoneButton.onClick.AddListener(Confirm);
        _CancelButton.onClick.AddListener(Cancel);
    }

    private void Cancel()
    {
        Close();
        _EditButton.gameObject.SetActive(true);
        _DeleteButton.gameObject.SetActive(true);
        _Text.gameObject.SetActive(true);
    }

    private void Close()
    {
        _EditIPF.text = string.Empty;
        _EditIPF.gameObject.SetActive(false);
        _DoneButton.gameObject.SetActive(false);
        _CancelButton.gameObject.SetActive(false);
    }

    private async void Confirm()
    {
        if (await WebRequest.UpdateContent(_Id, _EditIPF.text))
        {
            Close();
            UITODO.Instance.RefreshList();
        }
    }
    private void Edit()
    {
        _EditIPF.gameObject.SetActive(true);
        _DoneButton.gameObject.SetActive(true);
        _CancelButton.gameObject.SetActive(true);

        _EditButton.gameObject.SetActive(false);
        _DeleteButton.gameObject.SetActive(false);
        _Text.gameObject.SetActive(false);

        _EditIPF.ActivateInputField();

    }
}
