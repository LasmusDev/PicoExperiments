using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionnaireDummy : MonoBehaviour
{
    private GameObject defaultItem;
    private GameObject firstStage;
    void Start()
    {
        defaultItem = transform.Find("Item").gameObject;
        // defaultItem.SetActive(false);
        Questionnaire();
    }

    void Questionnaire()
    {
        firstStage = Instantiate(defaultItem, defaultItem.transform.position, defaultItem.transform.rotation, transform);
        firstStage.transform.position = new Vector3(firstStage.transform.position.x + 600, firstStage.transform.position.y, firstStage.transform.position.z);
        firstStage.SetActive(true);
        firstStage.transform.Find("Header Text").GetComponent<TMP_Text>().text = "Test";
        firstStage.transform.Find("Buttons/Submit").GetComponent<Button>().onClick.AddListener(() => Main.ChangeScene("00_Menu"));
    }
}
