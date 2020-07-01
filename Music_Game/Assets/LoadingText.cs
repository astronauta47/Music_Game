using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    float timer;
    [SerializeField] float speed;
    string startText;
    Text text;
    int index;

    private void Start()
    {
        text = GetComponent<Text>();
        startText = text.text;
    }

    void Update()
    {
        timer += Time.deltaTime * speed;

        if(timer > 1)
        {
            char[] chars = startText.ToCharArray();
            char[] newText = new char[chars.Length - index];

            for (int i = 0; i < chars.Length - index; i++)
            {
                newText[i] = chars[i];
            }

            text.text = new string(newText);

            index--;
            timer = 0;

            if (index < 0)
                index = 3;
        }
    }
}
