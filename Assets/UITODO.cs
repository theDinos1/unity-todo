using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static WebRequest;

public class UITODO : MonoBehaviour
{
    [SerializeField] private List<GameObject> _GameObjectList = new List<GameObject>();
    [SerializeField] private GameObject _TodoPrefab;
    [SerializeField] private Transform _ListParent;
    [SerializeField] private TMP_InputField _ContentIPF;
    [SerializeField] private Button _CreateButton;
    public static UITODO Instance;
    private List<Todos> _TodoList = new List<Todos>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        Init();
    }

    private void Init()
    {
        _CreateButton.onClick.AddListener(CreateContent);
        RefreshList();
    }

    private async void CreateContent()
    {
        if (string.IsNullOrEmpty(_ContentIPF.text)) return;
        bool success = await WebRequest.CreateContent(_ContentIPF.text);
        if (success) OnCreated();
        else Debug.Log("Failed");
    }

    private void OnCreated()
    {

        _ContentIPF.text = string.Empty;
        RefreshList();
    }

    public async void RefreshList()
    {
        foreach (var go in _GameObjectList) Destroy(go);
        _GameObjectList.Clear();
        if (_TodoList.Count > 0) _TodoList.Clear();
        _TodoList = await WebRequest.GetTodoList();
        foreach (var todo in _TodoList)
        {
            GameObject obj = Instantiate(_TodoPrefab, _ListParent);
            obj.GetComponent<TodoObject>().Setup(todo.content, todo.id);
            _GameObjectList.Add(obj);
        }

    }
}
