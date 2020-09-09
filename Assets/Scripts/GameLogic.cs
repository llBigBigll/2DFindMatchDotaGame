using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject timer;
    
    public GameObject SearchPanel;
    public GameObject TaskPanel;
    
    public TextAsset HeroListFile;

    public GameObject HeroPlatePrefab;

    private Text timerText;

    private List<string> searchingFieldList;
    private List<string> taskList;
    private List<string> foundList;
    private List<string> listOfNames;

    // Start is called before the first frame update
    void Start()
    {
        timerText = timer.GetComponentInChildren<Text>();
        listOfNames = getTexturesNames(HeroListFile);
        foundList = new List<string>();

        searchingFieldList = GetRandomElementsFromList(listOfNames, 8);
        taskList = GetRandomElementsFromList(searchingFieldList, 3);

        InstantiateSearchingList();
        InstantiateTaskList();
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
            heroCell.name = heroName;
            heroCell.transform.SetParent(SearchPanel.transform, false);
            heroCell.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
            var eventTriger = heroCell.transform.GetChild(0).GetComponent<EventTrigger>();
            AddEventTriggerListener(eventTriger, EventTriggerType.PointerClick, onPlateMouseDown);

        }
    }
    private void InstantiateTaskList() 
    {
        foreach (var heroName in taskList)
        {
            var sprite = Resources.Load<Sprite>($"HeroesPics/{heroName}");
            if (sprite == null) 
            {
                Debug.Log($"Не удалось найти текстуру с именем: \"{heroName}\"");
            }

            var heroCell = Instantiate(HeroPlatePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            heroCell.name = heroName;
            heroCell.transform.SetParent(TaskPanel.transform, false);
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

    public static void AddEventTriggerListener(EventTrigger trigger,
                                           EventTriggerType eventType,
                                           System.Action<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
        trigger.triggers.Add(entry);
    }

    public void onPlateMouseDown(BaseEventData data)
    {
        var other = (PointerEventData)data;
        GameObject background = other.pointerEnter;
        string name = other.pointerEnter.transform.parent.gameObject.name;
        if (taskList.Contains(name) && !foundList.Contains(name)) 
        {
            foundList.Add(name);
            var activeColor = Color.yellow;
            activeColor.a = 0.6f;
            background.GetComponent<Image>().color = activeColor;
        }
        if (!taskList.Contains(name)) 
        {
            var wrongColor = Color.red;
            wrongColor.a = 0.6f;
            background.GetComponent<Image>().color = wrongColor;
        }

        if (CheckWin()) 
        {
            ResetMg();
        }
    }

    private void ResetMg() 
    {
        for (int i = 0; i < TaskPanel.transform.childCount; i++)
        {
            Destroy(TaskPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < SearchPanel.transform.childCount; i++)
        {
            Destroy(SearchPanel.transform.GetChild(i).gameObject);
        }

        foundList = new List<string>();

        searchingFieldList = GetRandomElementsFromList(listOfNames, 8);
        taskList = GetRandomElementsFromList(searchingFieldList, 3);

        InstantiateSearchingList();
        InstantiateTaskList();
    }
    private bool CheckWin() 
    {
        bool answer = true;
        foreach (var task in taskList)
        {
            if (!foundList.Contains(task)) 
            {
                answer = false;
                break;
            }
        }
        return answer;
    }

}
