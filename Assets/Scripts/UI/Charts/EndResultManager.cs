using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndResultManager : MonoBehaviour
{
    [Header("LINES")]
    [SerializeField] private UIGridRenderer grid; 
    [SerializeField] private UILineRenderer redDaceLine;
    [SerializeField] private UILineRenderer chubLine;
    [SerializeField] private UILineRenderer troutLine;
    [SerializeField] private UILineRenderer tempLine;

    [Header("AREAS")]
    [SerializeField] private UIAreaRenderer redDaceArea;
    [SerializeField] private UIAreaRenderer chubArea;
    [SerializeField] private UIAreaRenderer troutArea;

    [Header("HOVERING")]
    [SerializeField] private RectTransform playerLine;
    [SerializeField] private RectTransform daceDot;
    [SerializeField] private RectTransform chubDot;
    [SerializeField] private RectTransform troutDot;

    [Header("SELECTED")]
    [SerializeField] private RectTransform selectedLine;
    [SerializeField] private RectTransform selectedDaceDot;
    [SerializeField] private RectTransform selectedChubDot;
    [SerializeField] private RectTransform selectedTroutDot;

    [Header("DISPLAY")]
    [SerializeField] private Slider tempSlider;
    [SerializeField] private TMP_Text tempNumber;
    [SerializeField] private Slider turbiditySlider;
    [SerializeField] private TMP_Text yearTitle;
    [SerializeField] private TMP_Text daceNumText;
    [SerializeField] private TMP_Text chubNumText;
    [SerializeField] private TMP_Text troutNumText;
    [SerializeField] private TMP_Text insectNumText;
    [SerializeField] private GameObject graphTempText;



    [SerializeField] private TMP_Text[] xAxisNumbers;
    [SerializeField] private TMP_Text[] yAxisNumbers;

    private List<float> redDaceValues = new List<float>();
    private float maxRedDaceValue = 0f;
    private float minRedDaceValue = 999999f;

    private List<float> chubValues = new List<float>();
    private float maxChubValue = 0f;
    private float minChubValue = 999999f;

    private List<float> troutValues = new List<float>();
    private float maxTroutValue = 0f;
    private float minTroutValue = 999999f;

    private List<float> insectValues = new List<float>();

    private List<float> globalTemps = new List<float>();
    private float maxTempValue = 30f;
    private float minTempValue = 0f;

    private List<float> globalTurbidity = new List<float>();

    private int graphSize;
    public int graphHeight = 25;

    [SerializeField] private RectTransform rect;

    [SerializeField] private int selectedYear=0;

    [Header("Card")]
    public List<CardInstance> cardInstances = new List<CardInstance>();
    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private TMP_Text cardDesc;
    [SerializeField] private TMP_Text cardStats;
    [SerializeField] private TMP_Text cardDuration;
    [SerializeField] private TMP_Text cardTileType;

    [SerializeField] private EndBarGraphManager barGraphManager;

    [Header("END GAME UI")]
    [SerializeField] private GameObject backButton;


    [Header("Togglable Bools")]
    public bool showChub = true;
    public bool showTrout = true;
    public bool showTemp = true;
    public bool showArea = true;


    public void CallStart()
    {
        grid.gridSize = new Vector2Int(10, graphHeight);
        redDaceLine.gridSize = new Vector2Int(10, graphHeight);
        chubLine.gridSize = new Vector2Int(10, graphHeight);
        troutLine.gridSize = new Vector2Int(10, graphHeight);

        redDaceLine.points.Clear();
        chubLine.points.Clear();
        troutLine.points.Clear();
        tempLine.points.Clear();
        //redDaceLine.points.Add(new Vector2(0f, 0f));
    }

    public void AddDataPoint(int year, float redDacePop, float chubPop, float troutPop, float insectPop, float temp, float turbidity)
    {    
        if (redDacePop > maxRedDaceValue)
        {
            maxRedDaceValue = redDacePop;
        }
        if (redDacePop < minRedDaceValue)
        {
            minRedDaceValue = redDacePop;
        }
        if (chubPop > maxChubValue)
        {
            maxChubValue = chubPop;
        }
        if (chubPop < minChubValue)
        {
            minChubValue = chubPop;
        }
        if (troutPop > maxTroutValue)
        {
            maxTroutValue = troutPop;
        }
        if (troutPop < minTroutValue)
        {
            minTroutValue = troutPop;
        }
        if (temp > maxTempValue)
        {
            maxTempValue = temp;

        }
        if (temp < minTempValue)
        {
            minTempValue = temp;
        }

        if (year % 10 == 0)
        {
            graphSize = year;
        }
        else
        {
            graphSize = (Mathf.FloorToInt(year / 10) + 1) * 10;
        }
        grid.gridSize = new Vector2Int(graphSize, graphHeight);
        redDaceLine.gridSize = new Vector2Int(graphSize, graphHeight);
        chubLine.gridSize = new Vector2Int(graphSize, graphHeight);
        troutLine.gridSize = new Vector2Int(graphSize, graphHeight);
        tempLine.gridSize = new Vector2Int(graphSize, graphHeight);
        redDaceArea.gridSize = new Vector2Int(graphSize, graphHeight);
        chubArea.gridSize = new Vector2Int(graphSize, graphHeight);
        troutArea.gridSize = new Vector2Int(graphSize, graphHeight);


        if (year == 1)
        {
            redDaceValues.Add(redDacePop);
            redDaceLine.points.Add(new Vector2(0, 0f));

            chubValues.Add(chubPop);
            chubLine.points.Add(new Vector2(0, 0f)); 
            
            troutValues.Add(troutPop);
            troutLine.points.Add(new Vector2(0, 0f));

            globalTemps.Add(temp);
            tempLine.points.Add(new Vector2(0, 0f));

            globalTurbidity.Add(turbidity);
        }
        redDaceValues.Add(redDacePop);
        redDaceLine.points.Add(new Vector2(year, 0f));

        chubValues.Add(chubPop);
        chubLine.points.Add(new Vector2(year, 0f));

        troutValues.Add(troutPop);
        troutLine.points.Add(new Vector2(year, 0f));

        globalTemps.Add(temp);
        tempLine.points.Add(new Vector2(year, 0f));

        globalTurbidity.Add(turbidity);

        insectValues.Add(insectPop);

        for (int i=0; i< xAxisNumbers.Length; i++)
        {
            xAxisNumbers[i].text = (i * (graphSize / 10)).ToString();
        }

        
        ShowSelectedYearData();
        selectedYear++;
        LayoutPoints();
    }

    private void LayoutPoints()
    {
        float minGraphValue = Mathf.Max(Mathf.FloorToInt(minRedDaceValue / 500) * 500, 0f);
        float maxGraphValue = Mathf.CeilToInt(maxRedDaceValue / 500) * 500;

        if (showChub)
        {
            minGraphValue = Mathf.Min(minGraphValue, (Mathf.Max(Mathf.FloorToInt(minChubValue / 500) * 500, 0f)));
            maxGraphValue = Mathf.Max(maxGraphValue, Mathf.CeilToInt(maxChubValue / 500) * 500);
        }

        if (showTrout)
        {
            minGraphValue = Mathf.Min(minGraphValue, (Mathf.Max(Mathf.FloorToInt(minTroutValue / 500) * 500, 0f)));
            maxGraphValue = Mathf.Max(maxGraphValue, Mathf.CeilToInt(maxTroutValue / 500) * 500);
        }

        yAxisNumbers[0].text = minGraphValue.ToString();
        yAxisNumbers[1].text = ((minGraphValue + maxGraphValue) / 2).ToString();
        yAxisNumbers[2].text = maxGraphValue.ToString();

        redDaceLine.gameObject.SetActive(false);
        chubLine.gameObject.SetActive(false);
        troutLine.gameObject.SetActive(false);
        tempLine.gameObject.SetActive(false);
        redDaceArea.gameObject.SetActive(false);
        chubArea.gameObject.SetActive(false);
        troutArea.gameObject.SetActive(false);

        for (int i = 0; i < redDaceLine.points.Count; i++)
        {
            redDaceLine.points[i] = new Vector2(i, (redDaceValues[i] - minGraphValue) / (maxGraphValue - minGraphValue) * graphHeight);//new Vector2((i*10f)/(float)graphSize, (redDaceValues[i]-minGraphValue)/(maxGraphValue-minGraphValue) *50);//redDaceValues[i]/

            if (showChub)
            {
                chubLine.points[i] = new Vector2(i, (chubValues[i] - minGraphValue) / (maxGraphValue - minGraphValue) * graphHeight);
            }

            if (showTrout)
            {
                troutLine.points[i] = new Vector2(i, (troutValues[i] - minGraphValue) / (maxGraphValue - minGraphValue) * graphHeight);
            }

            if (showTemp)
            {
                tempLine.points[i] = new Vector2(i, globalTemps[i] / maxTempValue * graphHeight);
            }

        }

        redDaceLine.gameObject.SetActive(true);
        if (showChub)
        {
            chubLine.gameObject.SetActive(true);
        }
        if (showTrout)
        {
            troutLine.gameObject.SetActive(true);
        }
        if (showTemp)
        {
            tempLine.gameObject.SetActive(true);
            graphTempText.SetActive(true);
        }
        else
        {
            graphTempText.SetActive(false);
        }

        redDaceArea.points = redDaceLine.points;
        chubArea.points = chubLine.points;
        troutArea.points = troutLine.points;

        redDaceArea.gameObject.SetActive(showArea);
        chubArea.gameObject.SetActive(showArea&&showChub);
        troutArea.gameObject.SetActive(showArea&&showTrout);

    }

    private void Update()
    {
        Vector2 mousePos = new Vector2();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(grid.rectTransform, Input.mousePosition, null, out mousePos);
        
        if(mousePos.x<0 || mousePos.y < 0 || mousePos.x>grid.rectTransform.rect.width || mousePos.y > grid.rectTransform.rect.height)
        {
            playerLine.gameObject.SetActive(false);
            return;
        }

        playerLine.gameObject.SetActive(true);


        float gapBetweenLines = grid.rectTransform.rect.width / grid.gridSize.x;

        float closestLine = Mathf.Round(mousePos.x / gapBetweenLines) * gapBetweenLines;

        playerLine.anchoredPosition = new Vector2(closestLine, 0f);

        int yearNum = Mathf.RoundToInt(closestLine / gapBetweenLines);
        if (redDaceLine.points.Count > yearNum)
        {
            daceDot.gameObject.SetActive(true);
            daceDot.anchoredPosition = new Vector2(0f, redDaceLine.points[yearNum].y * rect.rect.height / graphHeight);

            if (Input.GetMouseButtonDown(0))
            {
                selectedYear = yearNum;
                ShowSelectedYearData();
            }
        }
        else
        {
            daceDot.gameObject.SetActive(false);
        }

        if (chubLine.points.Count > yearNum && showChub)
        {
            chubDot.gameObject.SetActive(true);
            chubDot.anchoredPosition = new Vector2(0f, chubLine.points[yearNum].y * rect.rect.height / graphHeight);
        }
        else
        {
            chubDot.gameObject.SetActive(false);
        }

        if (troutLine.points.Count > yearNum && showTrout)
        {
            troutDot.gameObject.SetActive(true);
            troutDot.anchoredPosition = new Vector2(0f, troutLine.points[yearNum].y * rect.rect.height / graphHeight);
        }
        else
        {
            troutDot.gameObject.SetActive(false);
        }
    }

    public void ShowSelectedYearData()
    {
        float gapBetweenLines = grid.rectTransform.rect.width / grid.gridSize.x;
        selectedLine.anchoredPosition = new Vector2(selectedYear*gapBetweenLines, 0f);
        selectedDaceDot.anchoredPosition = new Vector2(0f, redDaceLine.points[selectedYear].y * rect.rect.height / graphHeight);
        selectedChubDot.anchoredPosition = new Vector2(0f, chubLine.points[selectedYear].y * rect.rect.height / graphHeight);
        selectedTroutDot.anchoredPosition = new Vector2(0f, troutLine.points[selectedYear].y * rect.rect.height / graphHeight);
        selectedChubDot.gameObject.SetActive(showChub);
        selectedTroutDot.gameObject.SetActive(showTrout);

        yearTitle.text = "Year " + selectedYear;
        tempSlider.value = globalTemps[selectedYear];
        tempNumber.text = globalTemps[selectedYear].ToString();
        turbiditySlider.value = globalTurbidity[selectedYear];

        if (selectedYear > 0 && cardInstances.Count>= selectedYear)
        {
            ShowCardInfo(selectedYear-1);

            daceNumText.text = redDaceValues[selectedYear - 1].ToString();
            chubNumText.text = chubValues[selectedYear-1].ToString();
            troutNumText.text = troutValues[selectedYear-1].ToString();
            insectNumText.text = insectValues[selectedYear-1].ToString();
        }

        barGraphManager.UpdateBarGraph();
    }

    private string Capitals = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private void ShowCardInfo(int cardIndex)
    {
        cardTitle.text = cardInstances[cardIndex].cardName;
        cardDesc.text = cardInstances[cardIndex].cardDescription;
        cardDuration.text = cardInstances[cardIndex].durationRemaining.ToString();

        string tileType = "";
        foreach (char letter in cardInstances[cardIndex].tileType)
        {
            if (Capitals.Contains(letter.ToString()) && tileType != "")
            {
                tileType += " ";
            }
            tileType += letter;
        }

        cardTileType.text = tileType;

        string stats = "<b>TILES AFFECTED:</b>\n";
        if (cardInstances[cardIndex].numberOfTiles == 0)
        {
            stats += " All water tiles";
        }
        else
        {
            stats += cardInstances[cardIndex].numberOfTiles + " tiles";
        }

        stats += "\n\n<b>DURATION OF EFFECT: </b>\n" + cardInstances[cardIndex].durationRemaining + " years";

        if (cardInstances[cardIndex].delayBeforeEffect != 0)
        {
            stats += "\n\n<b>DELAY BEFORE EFFECT: </b>\n " + cardInstances[cardIndex].delayBeforeEffect + " years";
        }
        else
        {
            stats += "\n\n<b>NO DELAY</b>";
        }

        cardStats.text = stats;
    }

    public void EndOfGame()
    {
        this.gameObject.SetActive(true);
        backButton.SetActive(false);
    }


    public void toggleChub()
    {
        showChub = !showChub;
        LayoutPoints();
    }
    public void toggleTrout()
    {
        showTrout = !showTrout;
        LayoutPoints();
    }

    public void toggleTemp()
    {
        showTemp = !showTemp;
        LayoutPoints();
    }

    public void toggleArea()
    {
        showArea = !showArea;
        LayoutPoints();
    }
}
