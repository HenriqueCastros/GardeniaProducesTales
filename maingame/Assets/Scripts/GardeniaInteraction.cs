using System.IO;
using System.Net;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

[System.Serializable]
public class GardeniaMsg
{
    public string MESSAGE;
}

public class GardeniaInteraction : MonoBehaviour
{
    public GameObject textPromt;

    private int initialState = 0;
    private List<string> states = new List<string> { "introduction", "prefight", "postfight", "preboss", "win", "death" };

    public void generateNewInfo()
    {
        textPromt.GetComponent<TMPro.TextMeshProUGUI>().text = "Let me think a little...";
        Task<string> getTask = GetAsync("http://127.0.0.1:5000/getmsg/?mode="+ states[initialState]);
        getTask.GetAwaiter().OnCompleted(() =>
       {
           GardeniaMsg result = UnityEngine.JsonUtility.FromJson<GardeniaMsg>(getTask.Result);
           textPromt.GetComponent<TMPro.TextMeshProUGUI>().text = result.MESSAGE;
           textPromt.GetComponent<UITextTypeWriter>().OnEnable();
       });
    }

    public string Get(string uri)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
    public async Task<string> GetAsync(string uri)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            return await reader.ReadToEndAsync();
        }
    }
}
