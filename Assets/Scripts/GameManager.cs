using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static float fl_health = 100f;
    public static float fl_time = 12;
    public static float fl_days = 30;
    public bool toggleList = true;
    public bool toggleInv = false;
    public bool toggleFlash = true;
    public int hungerRate = 3;
    public int thirstRate = 2;
    public int shelterRate = 2;    
    private int in_compObj = 0;
    private int inv_food = 0;
    private int inv_water = 0;
    private int inv_wood = 0;
    private int inv_medkit = 1;
    private bool bl_night;
    private bool bl_fire;
    private bool bl_sheltered;
    private int multiplyRate = 3;
    private float lerpTime = 0;
    private float lerpObject = 0;
    private Button checklistButton;
    private Button foodButton;
    private Button waterButton;
    private Button medkitButton;
    private Text playerDays;
    private Text playerTime;
    private Text playerTimeCycle;
    private Text inventoryList;
    private Text completeObjectives;
    private Text fireCheck;
    private Text foodCheck;
    private Text waterCheck;
    private Text woodCheck;
    private Text invFoodText;
    private Text invWaterText;
    private Text invMedkitText;
    private Text invWoodText;
    private Text checkList;
    private Text days;
    private Text tutText;
    private Slider hungerSlider;
    private Slider thirstSlider;
    private Slider shelterSlider;
    private Text HUD_health;
    private Text infoNote;
    private string st_game_health;
    private string st_note_text = "";
    private string toolTip = "";
    private string tut_message = "";
    private GameObject tutorial;
    private GameObject GO_PC;
    private GameObject GO_Flashlight;
    private GameObject GO_Map;
    private GameObject GO_Death;
    private PlayerMovement pm;
    private RotateToCursor rc;
    
    void Start()
    {
        
        HUD_health = GameObject.Find("Health").GetComponent<Text>();
        hungerSlider = GameObject.Find("HungerSlider").GetComponent<Slider>();
        thirstSlider = GameObject.Find("ThirstSlider").GetComponent<Slider>();
        shelterSlider = GameObject.Find("ShelterSlider").GetComponent<Slider>();
        checklistButton = GameObject.Find("ChecklistButton").GetComponent<Button>();
        foodButton = GameObject.Find("UseFood").GetComponent<Button>();
        waterButton = GameObject.Find("UseWater").GetComponent<Button>();
        medkitButton = GameObject.Find("UseMedkit").GetComponent<Button>();
        playerDays = GameObject.Find("Days").GetComponent<Text>();
        playerTime = GameObject.Find("Time").GetComponent<Text>();
        playerTimeCycle = GameObject.Find("cycle").GetComponent<Text>();
        inventoryList = GameObject.Find("Inventory").GetComponent<Text>();        
        invFoodText = GameObject.Find("FoodItems").GetComponent<Text>();
        invWaterText = GameObject.Find("WaterItems").GetComponent<Text>();
        invMedkitText = GameObject.Find("MedkitItems").GetComponent<Text>();
        invWoodText = GameObject.Find("WoodItems").GetComponent<Text>();
        checkList = GameObject.Find("Checklist").GetComponent<Text>();
        days = GameObject.Find("Days").GetComponent<Text>();
        foodCheck = checkList.transform.Find("GetFood").GetComponent<Text>();
        waterCheck = checkList.transform.Find("GetWater").GetComponent<Text>();
        fireCheck = checkList.transform.Find("GetFire").GetComponent<Text>();
        woodCheck = checkList.transform.Find("GetWood").GetComponent<Text>();
        completeObjectives = GameObject.Find("CompletedObjectives").GetComponent<Text>();
        infoNote = GameObject.Find("Note").GetComponent<Text>();
        tutorial = GameObject.Find("Tutorial");
        tutText = GameObject.Find("Speach").GetComponent<Text>();
        GO_PC = GameObject.Find("Player");
        GO_Flashlight = GameObject.Find("Flashlight");
        GO_Map = GameObject.Find("Map");
        GO_Death = GameObject.Find("GameOver");
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        rc = GameObject.Find("Player").GetComponent<RotateToCursor>();
        
        UpdatePCStats();
        StartTutorial();    
    }

    void Update()
    {       
        DayNightCycle();
        WorldInteraction();
        UpdateUI();
        OpenInventory();
        EnableFlash();
        CheckSliders();
        CheckDeath();
    }

    void OnGUI()
    {
        if (toolTip != "")
        {
            GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y, 400, 20), toolTip);
        }       
    }

    void UpdateUI()
    {
        if (fl_time <= 9)
        {
            playerTime.text = "Time:  0" + fl_time;
        }
        else
        {
            playerTime.text = "Time:  " + fl_time;
        }

        completeObjectives.text = in_compObj + " complete";
        playerDays.text = "Days Remaining: " + fl_days;
        invFoodText.text = "Food = " + inv_food + " items";
        invWaterText.text = "Water = " + inv_water + " items";
        invMedkitText.text = "Medkit = " + inv_medkit + " items";
        invWoodText.text = "Wood = " + inv_wood + " items";
        infoNote.text = st_note_text;
    }

    void UpdatePCStats()
    {
        StartCoroutine("UpdateTime");
        StartCoroutine("DecreaseHunger");
        StartCoroutine("DecreaseThirst");
        CallDecreaseShelter();       
    }

    void StartTutorial()
    {
        GO_Death.SetActive(false);
        GO_PC.SetActive(false);
        GO_Flashlight.SetActive(false);
        checkList.gameObject.SetActive(false);
        days.gameObject.SetActive(false);
        tut_message = tutText.text;
        tutText.text = "";
        StartCoroutine(TypeText());
    }

    void DayNightCycle()
    {

        GO_Map.transform.GetComponent<Renderer>().material.color = Color32.Lerp(new Color32(255, 255, 255, 255), new Color32(39, 39, 39, 255), Mathf.PingPong(Time.time / 60, 1));

        if (fl_time >= 6) 
        {
            bl_night = false;
            playerTimeCycle.text = "Currently Day";
        }

        if (fl_time >= 21)
        {
            bl_night = true;
            playerTimeCycle.text = "Currently Night";
        }
        
    }

    void WorldInteraction()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit)
            {

                switch (hit.collider.tag)
                {
                    case "Shelter":

                        StartCoroutine("ShowText", "You selected " + hit.collider.name);

                        break;

                    case "Fire":

                        if (!bl_fire)
                        {
                            if (inv_wood < 10)
                            {
                                StartCoroutine("ShowText", "You need 10 wood items to light a fire!");
                            }
                            else
                            {
                                inv_wood -= 10;
                                if (fireCheck.text == "- Start Fire")
                                {
                                    in_compObj += 1;
                                    fireCheck.color = Color.green;
                                    fireCheck.text = "- Start Fire (Complete)";
                                }
                                StartCoroutine("ShowText", "Lighting Fire...");
                                bl_fire = true;

                            }                            
                        }
                        else
                        {
                            StartCoroutine("ShowText", "You selected " + hit.collider.name);
                        }
                        break;

                    case "Oak Wood":

                        StartCoroutine("ShowText", "Can't use Oak Wood to light a fire");
                        break;

                    case "Ocean Water":

                        StartCoroutine("ShowText", "You drank " + hit.collider.name);
                        if (fl_health > 25)
                        {
                            fl_health -= 25;
                        }
                        else
                        {
                            fl_health = 0;
                        }
                        
                        break;
                }
            }
        }
        else if (hit)
        {
            switch (hit.collider.tag)
            {
                case "Shelter":

                    toolTip = "Shelter Area";
                    break;

                case "Fire":
                    
                    toolTip = "Fireplace";
                    break;

                case "Food":
                    
                    toolTip = "Hunt Food";
                    break;

                case "Water":

                    toolTip = "Clean Water";
                    break;

                case "Slow Terrain":

                    toolTip = "Slow Terrain";
                    break;

                case "Ocean Water":

                    toolTip = "Ocean Water";
                    break;

                case "Wood":

                    toolTip = "Forest Wood";
                    break;

                case "Oak Wood":

                    toolTip = "Oak Wood";
                    break;  
               
            }

        }
        else if(!hit)
        {
            toolTip = "";
        }
    }    

    void CheckSliders()
    {
        st_game_health = "Health     " + fl_health.ToString("F0");
        HUD_health.text = st_game_health;    

        if (hungerSlider.value <= 0)
        {
            if (fl_health > 0)
            {
                fl_health -= 0.05f;
            }
            
        }

        if (thirstSlider.value <= 0)
        {
            if (fl_health > 0)
            {
                fl_health -= 0.05f;
            }
        }

        if (shelterSlider.value <= 0)
        {
            if (fl_health > 0)
            {
                fl_health -= 0.05f;
            }
        }

        if (hungerSlider.value >= 0.8f)
        {
            if (fl_health < 100)
            {
                fl_health += 0.05f;
            }

        }

        if (thirstSlider.value >= 0.8f)
        {
            if (fl_health < 100)
            {
                fl_health += 0.05f;
            }
        }

        if (shelterSlider.value >= 0.8f)
        {
            if (fl_health < 100)
            {
                fl_health += 0.05f;
            }
        }

    }

    void CheckDeath()
    {
        if (fl_health <= 0)
        {
            pm.bl_canMove = false;
            rc.bl_canRotate = false;
            GO_Death.SetActive(true);
            
        }
    }

    void DisplayChecklist()
    {        
        if (toggleList)
        {
            checklistButton.GetComponentInChildren<Text>().text = "Hide Checklist";
            checkList.gameObject.SetActive(true);
        }         
        else
        {
            checklistButton.GetComponentInChildren<Text>().text = "Show Checklist";
            checkList.gameObject.SetActive(false);
        }

        toggleList = !toggleList;
    }

    void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            lerpObject = 0;

            if (!toggleInv)
            {
                StopCoroutine("ShowInventory");         
                StartCoroutine("HideInventory");                          
            }
            else
            {
                StopCoroutine("HideInventory");
                StartCoroutine("ShowInventory");
            }

            toggleInv = !toggleInv;
        }
    }

    void EnableFlash()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (toggleFlash)
            {
                GO_Flashlight.SetActive(false);
            }
            else
            {
                GO_Flashlight.SetActive(true);
            }

            toggleFlash = !toggleFlash;
        }
    }

    public void GetFood(int foodItem)
    {              
        if (foodItem == 1)
        {
            StartCoroutine("ShowText", "You collected food");
            inv_food += foodItem;
            if (foodCheck.text == "- First Time Collecting Food")
            {
                in_compObj += 1;
                foodCheck.color = Color.green;
                foodCheck.text = "- First Time Collecting Food (Complete)";
            }          
        }
        else
        {
            StartCoroutine("ShowText", "No more food to collect");
        }
    }

    public void GetWater(int waterItem)
    {        
        if (waterItem == 1)
        {
            StartCoroutine("ShowText", "You collected water");
            inv_water += waterItem;
            if (waterCheck.text == "- First Time Collecting Water")
            {
                in_compObj += 1;
                waterCheck.color = Color.green;
                waterCheck.text = "- First Time Collecting Water (Complete)";
            }
        }
        else
        {
            StartCoroutine("ShowText", "No more water to collect");
        }
        
    }

    public void GetWood(int woodItem)
    {
        if (woodItem == 1)
        {
            StartCoroutine("ShowText", "You collected wood");
            inv_wood += woodItem;
            if (woodCheck.text == "- First Time Collecting Wood")
            {
                in_compObj += 1;
                woodCheck.color = Color.green;
                woodCheck.text = "- First Time Collecting Wood (Complete)";
            }
        }
        else
        {
            StartCoroutine("ShowText", "No more wood to collect");
        }
    }

    public void UseFoodButton()
    {
        if (inv_food > 0)
        {
            CallIncreaseHunger();            
        }
    }

    public void UseWaterButton()
    {
        if (inv_water > 0)
        {
            CallIncreaseThirst();            
        }
    }

    public void UseMedkitButton()
    {
        if (inv_medkit > 0)
        {
            CallIncreaseHealth();            
        }
    }

    public void CloseTutorial()
    {        
        GO_PC.SetActive(true);
        checkList.gameObject.SetActive(true);
        days.gameObject.SetActive(true);
        tutorial.SetActive(false);
    }

    public void CallIncreaseHealth()
    {
        if (fl_health < 75)
        {
            fl_health += 25;
            inv_medkit -= 1;
        }

    }

    public void CallIncreaseHunger()
    {
        if (hungerSlider.value <= 1)
        {
            hungerSlider.value += 0.2f;
            inv_food -= 1;
        }
       
    }

    public void CallIncreaseThirst()
    {
        if (thirstSlider.value <= 1)
        {
            thirstSlider.value += 0.2f;
            inv_water -= 1;
        }
        
    }

    public void CallIncreaseShelter()
    {
        bl_sheltered = true;
        StartCoroutine("IncreaseShelter");
    }

    public void CallDecreaseShelter()
    {
        bl_sheltered = false;        
        StartCoroutine("DecreaseShelter");       
    }

    IEnumerator ShowInventory()
    {        
        while (lerpObject < 1)
        {
            lerpObject += Time.deltaTime * 2;
            inventoryList.GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(-117, -247, 1), new Vector3(-117, 156, 1), lerpObject);

            yield return null;
        }
    }

    IEnumerator HideInventory()
    {        
        while (lerpObject < 1)
        {
            lerpObject += Time.deltaTime * 2;
            inventoryList.GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(-117, 156, 1), new Vector3(-117, -247, 1), lerpObject);

            yield return null;
        }
    }

    IEnumerator UpdateTime()
    {
        while (fl_time < 24)
        {
            yield return new WaitForSeconds(5);
            fl_time += 1;
        }

        if (fl_time == 24)
        {
            fl_days -= 1;
            fl_time = 0;
            StartCoroutine("UpdateTime");
        }
    }

    IEnumerator IncreaseShelter()
    {
        while (shelterSlider.value <= 1 && bl_sheltered)
        {
            yield return new WaitForSeconds(shelterRate);
            if (bl_fire)
            {
                shelterSlider.value = 1f;
            }
            else if (!bl_fire)
            {
                shelterSlider.value += 0.05f;
            }
        }

    }

    IEnumerator DecreaseShelter()
    {
        while (shelterSlider.value >= 0 && !bl_sheltered)
        {
            yield return new WaitForSeconds(shelterRate);           

            if (bl_night)
            {
                shelterSlider.value -= 0.04f;
            }
            else if (!bl_night)
            {
                shelterSlider.value -= 0.01f;
            }
        }

    }

    IEnumerator DecreaseHunger()
    {
        while (hungerSlider.value >= 0)
        {
            while (bl_sheltered)
            {
                yield return null;
            }

            yield return new WaitForSeconds(hungerRate);
            hungerSlider.value -= 0.02f;            
        }
        
    }

    IEnumerator DecreaseThirst()
    {
        while (thirstSlider.value >= 0)
        {
            while (bl_sheltered)
            {
                yield return null;
            }

            yield return new WaitForSeconds(thirstRate);
            thirstSlider.value -= 0.02f;
        }

    }

    IEnumerator ShowText(string text)
    {
        st_note_text = text;
        yield return new WaitForSeconds(2);
        st_note_text = "";

    }

    IEnumerator TypeText()
    {
        foreach (char letter in tut_message.ToCharArray())
        {
            tutText.text += letter;            
            yield return new WaitForSeconds(0.01f);
        }
    }

}


