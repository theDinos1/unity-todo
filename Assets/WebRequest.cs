using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

public static class WebRequest
{
    public class Response
    {
        public bool status;
        public object result;
    }
    public class Todos
    {
        public uint id;
        public string content;
    }
    public static async Task<List<Todos>> GetTodoList()
    {
        string response = await Get("http://localhost:3000/api/v1/todo/getAll");
        try
        {
            Response res = JsonConvert.DeserializeObject<Response>(response);
            if (!res.status) return null;
            return JsonConvert.DeserializeObject<List<Todos>>(res.result.ToString());
        }
        catch (Exception err)
        {
            UnityEngine.Debug.LogError(err.Message);
            return null;
        }

    }
    public static async Task<bool> CreateContent(string text)
    {
        var jsonData = new
        {
            content = text
        };
        UnityEngine.Debug.Log(JsonConvert.SerializeObject(jsonData));
        string response = await Post("http://localhost:3000/api/v1/todo/create", JsonConvert.SerializeObject(jsonData));
        UnityEngine.Debug.Log(response);
        try
        {
            Response res = JsonConvert.DeserializeObject<Response>(response);
            return res.status;
        }
        catch (Exception err)
        {
            UnityEngine.Debug.LogError(err.Message);
            return false;
        }

    }
    public static async Task<bool> RemoveContent(uint id)
    {
        var jsonData = new
        {
            id = id
        };
        UnityEngine.Debug.Log(JsonConvert.SerializeObject(jsonData));
        string response = await Post("http://localhost:3000/api/v1/todo/remove", JsonConvert.SerializeObject(jsonData));
        UnityEngine.Debug.Log(response);
        try
        {
            Response res = JsonConvert.DeserializeObject<Response>(response);
            return res.status;
        }
        catch (Exception err)
        {
            UnityEngine.Debug.LogError(err.Message);
            return false;
        }

    }
    public static async Task<bool> UpdateContent(uint id,string text)
    {
        var jsonData = new
        {
            id = id,
            content = text
        };
        UnityEngine.Debug.Log(JsonConvert.SerializeObject(jsonData));
        string response = await Post("http://localhost:3000/api/v1/todo/update", JsonConvert.SerializeObject(jsonData));
        UnityEngine.Debug.Log(response);
        try
        {
            Response res = JsonConvert.DeserializeObject<Response>(response);
            return res.status;
        }
        catch (Exception err)
        {
            UnityEngine.Debug.LogError(err.Message);
            return false;
        }

    }
    public static async Task<string> Get(string url, Dictionary<string, string> headers = null)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        if (headers != null)
        {
            foreach (KeyValuePair<string, string> kvp in headers)
            {
                request.SetRequestHeader(kvp.Key, kvp.Value);
            }
        }
        request.SetRequestHeader("Content-Type", "application/json");

        var uwr = request.SendWebRequest();
        while (!uwr.isDone) await Task.Yield();
        if (request.result == UnityWebRequest.Result.Success) return request.downloadHandler.text;
        else return request.error;

    }
    public static async Task<string> Post(string url, string jsonData = "{}", Dictionary<string, string> headers = null)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        if (headers != null)
        {
            foreach (KeyValuePair<string, string> kvp in headers)
            {
                request.SetRequestHeader(kvp.Key, kvp.Value);
            }
        }
        request.SetRequestHeader("Content-Type", "application/json");

        var uwr = request.SendWebRequest();
        while (!uwr.isDone) await Task.Yield();
        if (request.result == UnityWebRequest.Result.Success) return request.downloadHandler.text;
        else return request.error;

    }
}
