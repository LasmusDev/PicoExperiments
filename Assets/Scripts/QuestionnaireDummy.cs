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
        var firstStage = Instantiate(defaultItem);
        firstStage.transform.Find("HeaderText").GetComponent<TMP_Text>().text = "Test";
        firstStage.SetActive(true);
        firstStage.transform.Find("Submit").GetComponent<Button>().onClick.AddListener(() => Main.ChangeScene("00_Menu"));
    }

    void Update()
    {

    }
}
