using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject timer;
    
    public GameObject SearchPanel;
    
    public TextAsset HeroListFile;

    public GameObject HeroPlatePrefab;

    private Text timerText;

    private List<string> searchingFieldList;

    // Start is called before the first frame update
    void Start()
    {
        timerText = timer.GetComponentInChildren<Text>();
        List<string> listOfNames = getTexturesNames(HeroListFile);

        searchingFieldList = GetRandomElementsFromList(listOfNames,3);
        InstantiateSearchingList();
    }

    // Update is called once per frame
    void Update()
    {
        SetTimeText(timerText);
    }

    private void SetTimeText(Text textHandler, float beginTime = 0f)
    {
        var min = (int)((Time.time - beginTime) / 60f);
        var sec = (int)((Time.time - beginTime) % 60f);
        textHandler.text = min.ToString("00") + ":" + sec.ToString("00");
    }

    public static List<string> getTexturesNames(TextAsset textFile)
    {
        List<string> answer = new List<string>();
        var fileNames = textFile.text.Split(new char[] { ' ', '\n' });
        foreach (var fileName in fileNames)
        {
            var trimmed = fileName.Trim();
            answer.Add(trimmed);
        }
        return answer;
    }

    private void InstantiateSearchingList() 
    {
        foreach (var heroName in searchingFieldList)
        {
            var sprite = Resources.Load<Sprite>($"HeroesPics/{heroName}");
            if (sprite == null) 
            {
                Debug.Log($"Не удалось найти текстуру с именем: \"{heroName}\"");
            }

            var heroCell = Instantiate(HeroPlatePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            
            heroCell.transform.SetParent(SearchPanel.transform, false);
            heroCell.transform.GetChild(1).GetComponent<Image>().sprite = sprite;

        }
    }
    private static List<T> GetRandomElementsFromList<T>(List<T> list, int numberOfElements, bool canRepeat = false) 
    {
        var copy = new List<T>(list);

        List<T> answer = new List<T>();

        var count = copy.Count;

        if (numberOfElements > count && !canRepeat) 
        {
            numberOfElements = count;
        }

        for (int i = 0; i < numberOfElements; i++)
        {
            while (true) 
            {
                var randElem = Random.Range(0, copy.Count);
                if ((!answer.Contains(copy[randElem])) || canRepeat) 
                {
                    answer.Add(copy[randElem]);
                    copy.RemoveAt(randElem);
                    break;
                }
            }
        }

        return answer;
    }

}
