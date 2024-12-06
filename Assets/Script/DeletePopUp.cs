using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeletePopUp : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text text;
    void Start()
    {
        text.SetText("Your account is deleted");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
