using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject CanvasManager;

    public GameObject timerHandler;
    public GameObject scoreHandler;
    public GameObject failureHandler;

    public GameObject SearchPanel;
    public GameObject TaskPanel;

    public int TaskSize;
    public int SearchListSize;

    public TextAsset HeroListFile;

    public GameObject HeroPlatePrefab;

    private float startTimeGame;
    private float startTimeRound;

    private Text timerText;
    private Text scoreText;
    private Text failureText;

    private List<string> searchingFieldList;
    private List<string> taskList;
    private List<string> foundList;
    private List<string> listOfNames;

    private int score;
    private int failure;

    private bool isInGame;

    private static Color transparent = new Color(0f, 0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        timerText = timerHandler.GetComponentInChildren<Text>();
        scoreText = scoreHandler.GetComponentInChildren<Text>();
        failureText = failureHandler.GetComponentInChildren<Text>();

        SetScore(0);

        SetFailure(0);

        listOfNames = getTexturesNames(HeroListFile);
        foundList = new List<string>();

        searchingFieldList = GetRandomElementsFromList(listOfNames, SearchListSize);
        taskList = GetRandomElementsFromList(searchingFieldList, TaskSize);

        InstantiateSearchingList();
        InstantiateTaskList();

        TaskPanel.GetComponent<Image>().color = transparent;

        isInGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInGame) 
        {
            SetTimeText(timerText);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isInGame = false;
                CanvasManager.GetComponent<CanvasManager>().SetToMainMenu();
            }
        }
    }

    private void SetTimeText(Text textHandler)
    {
        var min = (int)((Time.time - startTimeGame) / 60f);
        var sec = (int)((Time.time - startTimeGame) % 60f);
        textHandler.text = min.ToString("00") + ":" + sec.ToString("00");
    }

    private void ChangeScore(int changeValue)
    {
        score += changeValue;
        scoreText.text = $"Score: {score}";
    }

    private void SetScore(int settingValue)
    {
        score = settingValue;
        scoreText.text = $"Score: {score}";
    }

    private void ChangeFailure(int changeValue)
    {
        failure += changeValue;
        failureText.text = $"Failure: {failure}";
    }

    private void SetFailure(int settingValue)
    {
        failure = settingValue;
        failureText.text = $"Failure: {failure}";
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
            TaskPanel.transform.GetChild(0).GetComponent<Text>().text += heroName + "\n";
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

    public void OnReserBttnClick()
    {
        RestartMg();
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
            background.GetComponent<Image>().raycastTarget = false;
            ChangeScore(1);
        }
        if (!taskList.Contains(name)) 
        {
            var wrongColor = Color.red;
            wrongColor.a = 0.6f;
            background.GetComponent<Image>().color = wrongColor;
            background.GetComponent<Image>().raycastTarget = false;
            ChangeScore(-1);
            ChangeFailure(-1);
        }

        if (CheckWin()) 
        {
            ResetRound();
        }
    }

    private void ResetRound()
    {
        startTimeRound = Time.time;

        TaskPanel.transform.GetChild(0).GetComponent<Text>().text = "";

        for (int i = 0; i < SearchPanel.transform.childCount; i++)
        {
            Destroy(SearchPanel.transform.GetChild(i).gameObject);
        }

        foundList = new List<string>();

        searchingFieldList = GetRandomElementsFromList(listOfNames, SearchListSize);
        taskList = GetRandomElementsFromList(searchingFieldList, TaskSize);

        InstantiateSearchingList();
        InstantiateTaskList();
    }

    private void ResetMg() 
    {
        startTimeGame = Time.time;
        startTimeRound = Time.time;

        SetScore(0);
        SetFailure(0);

        for (int i = 0; i < TaskPanel.transform.childCount; i++)
        {
            TaskPanel.transform.GetChild(0).GetComponent<Text>().text = "";
        }

        for (int i = 0; i < SearchPanel.transform.childCount; i++)
        {
            Destroy(SearchPanel.transform.GetChild(i).gameObject);
        }

        foundList = new List<string>();

        searchingFieldList = GetRandomElementsFromList(listOfNames, SearchListSize);
        taskList = GetRandomElementsFromList(searchingFieldList, TaskSize);

        InstantiateSearchingList();
        InstantiateTaskList();
    }

    public void RestartMg() 
    {
        isInGame = true;
        ResetMg();
    }
    public void RestartRound()
    {
        isInGame = true;
        ResetRound();
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
