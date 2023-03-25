using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionnaireDummy : MonoBehaviour
{
    private GameObject defaultItem;
    void Start()
    {
        defaultItem = transform.Find("Item").gameObject;
        defaultItem.SetActive(false);
        Questionnaire();
    }

    void Questionnaire()
    {
        GameObject firstStage = Instantiate(defaultItem, defaultItem.transform.position, defaultItem.transform.rotation, transform);
        firstStage.SetActive(true);
        firstStage.transform.Find("Header Text").GetComponent<TMP_Text>().text = "Test";
        firstStage.transform.Find("Buttons/Submit").GetComponent<Button>().onClick.AddListener(() => Main.ChangeScene("00_Menu"));
    }

    void Update()
    {

    }
}
