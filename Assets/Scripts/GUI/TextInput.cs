using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextInput : MonoBehaviour
{
    int maxTextInput = 20;
    int maxNameInput = 12;
    int maxNumberInput = 8;

    public void TextLetterClicked()
    {
        if (gameObject.name != "spaceButton")
        {
            GameManager.instance.letterButtonPressed = true;
            AddCharToTextInput(gameObject.transform.GetChild(0).GetComponent<Text>().text);
        } else
        {
            AddCharToTextInput(" ");
        }
    }

    void AddCharToTextInput(string letter)
    {
        Text inputText = GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>();
        string inputString = inputText.text;

        if (inputString.Length < maxTextInput) //only allows maxTextInput chars
        {
            inputString = inputString + letter;
            inputText.text = inputString;
        }
    }

    public void TextConfirmClicked()
    {
        if (GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text.Length > 0)
        {
            GameManager.instance.textInput = GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text;
        }
    }

    public void TextBackspaceClicked()
    {
        string value = GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text;
        if (value.Length > 0)
        {
            value = value.Substring(0, value.Length - 1);
            GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text = value;
        }
    }

    public void TextClearClicked()
    {
        GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text = "";
    }

    public void TextShiftClicked()
    {
        StartCoroutine(TextShiftCoroutine());
    }

    IEnumerator TextShiftCoroutine()
    {
        GameManager.instance.letterButtonPressed = false;

        GameObject panel = GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/LetterButtonsPanel");

        if (GameManager.instance.capsOn)
        {
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToLower();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "1";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "2";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "3";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "4";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "5";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "6";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "7";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "8";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "9";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "0";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "-";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "'";
                    }
                }
            }
        }
        else
        {
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToUpper();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "!";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "@";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "#";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "$";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "%";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "^";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "&";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "*";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "(";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ")";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "_";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ".";
                    }
                }
            }
        }

        while (GameManager.instance.letterButtonPressed == false)
        {
            yield return null;
        }

        if (!GameManager.instance.capsOn)
        {
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToLower();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "1";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "2";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "3";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "4";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "5";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "6";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "7";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "8";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "9";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "0";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "-";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "'";
                    }
                }
            }
        }
        else
        {
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToUpper();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "!";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "@";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "#";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "$";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "%";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "^";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "&";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "*";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "(";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ")";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "_";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ".";
                    }
                }
            }
        }

    }

    public void TextCapsClicked()
    {
        GameManager.instance.capsOn = !GameManager.instance.capsOn;

        GameObject panel = GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/LetterButtonsPanel");

        if (!GameManager.instance.capsOn)
        {
            GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/OptionsPanel/capsButton/capsText").GetComponent<Text>().fontStyle = FontStyle.Normal;
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToLower();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "1";
                    } else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "2";
                    } else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "3";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "4";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "5";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "6";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "7";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "8";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "9";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "0";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "-";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "'";
                    }
                }
            }
        } else
        {
            GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/OptionsPanel/capsButton/capsText").GetComponent<Text>().fontStyle = FontStyle.Bold;
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToUpper();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "!";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "@";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "#";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "$";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "%";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "^";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "&";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "*";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "(";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ")";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "_";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ".";
                    }
                }
            }
        }
    }


    //-- Name Input
    public void NameLetterClicked()
    {
        if (gameObject.name != "spaceButton")
        {
            GameManager.instance.letterButtonPressed = true;
            AddCharToNameInput(gameObject.transform.GetChild(0).GetComponent<Text>().text);
        }
        else
        {
            AddCharToNameInput(" ");
        }
    }

    void AddCharToNameInput(string letter)
    {
        Text inputText = GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>();
        string inputString = inputText.text;

        if (inputString.Length < maxNameInput) //only allows maxNameInput chars
        {
            inputString = inputString + letter;
            inputText.text = inputString;
        }
    }

    public void NameConfirmClicked()
    {
        if (GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text.Length > 0)
        {
            GameManager.instance.nameInput = GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text;
        }
    }

    public void NameBackspaceClicked()
    {
        string value = GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text;
        if (value.Length > 0)
        {
            value = value.Substring(0, value.Length - 1);
            GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text = value;
        }
    }

    public void NameClearClicked()
    {
        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text = "";
    }

    public void NameShiftClicked()
    {
        StartCoroutine(NameShiftCoroutine());
    }

    IEnumerator NameShiftCoroutine()
    {
        GameManager.instance.letterButtonPressed = false;

        GameObject panel = GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/LetterButtonsPanel");

        if (GameManager.instance.capsOn)
        {
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToLower();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "1";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "2";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "3";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "4";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "5";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "6";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "7";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "8";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "9";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "0";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "-";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "'";
                    }
                }
            }
        }
        else
        {
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToUpper();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "!";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "@";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "#";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "$";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "%";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "^";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "&";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "*";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "(";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ")";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "_";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ".";
                    }
                }
            }
        }

        while (GameManager.instance.letterButtonPressed == false)
        {
            yield return null;
        }

        if (!GameManager.instance.capsOn)
        {
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToLower();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "1";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "2";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "3";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "4";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "5";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "6";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "7";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "8";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "9";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "0";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "-";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "'";
                    }
                }
            }
        }
        else
        {
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToUpper();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "!";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "@";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "#";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "$";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "%";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "^";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "&";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "*";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "(";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ")";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "_";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ".";
                    }
                }
            }
        }

    }

    public void NameCapsClicked()
    {
        GameManager.instance.capsOn = !GameManager.instance.capsOn;

        GameObject panel = GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/LetterButtonsPanel");

        if (!GameManager.instance.capsOn)
        {
            GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/OptionsPanel/capsButton/capsText").GetComponent<Text>().fontStyle = FontStyle.Normal;
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToLower();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "1";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "2";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "3";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "4";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "5";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "6";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "7";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "8";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "9";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "0";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "-";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "'";
                    }
                }
            }
        }
        else
        {
            GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/OptionsPanel/capsButton/capsText").GetComponent<Text>().fontStyle = FontStyle.Bold;
            foreach (Transform childButton in panel.transform)
            {
                if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                    childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                    childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToUpper();
                }
                else
                {
                    if (childButton.name == "1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "!";
                    }
                    else if (childButton.name == "2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "@";
                    }
                    else if (childButton.name == "3Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "#";
                    }
                    else if (childButton.name == "4Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "$";
                    }
                    else if (childButton.name == "5Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "%";
                    }
                    else if (childButton.name == "6Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "^";
                    }
                    else if (childButton.name == "7Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "&";
                    }
                    else if (childButton.name == "8Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "*";
                    }
                    else if (childButton.name == "9Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "(";
                    }
                    else if (childButton.name == "0Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ")";
                    }
                    else if (childButton.name == "char1Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = "_";
                    }
                    else if (childButton.name == "char2Button")
                    {
                        childButton.GetChild(0).GetComponent<Text>().text = ".";
                    }
                }
            }
        }
    }

    //-- Number Input

    public void NumberClicked()
    {
        AddNumberInput(gameObject.transform.GetChild(0).GetComponent<Text>().text);
    }

    void AddNumberInput(string number)
    {
        string currentNumber = GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel/NumberEnteredPanel/NumberEnteredText").GetComponent<Text>().text;
        if (currentNumber == "0")
        {
            GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel/NumberEnteredPanel/NumberEnteredText").GetComponent<Text>().text = number;
        } else
        {
            if (currentNumber.Length < maxNumberInput)
            {
                GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel/NumberEnteredPanel/NumberEnteredText").GetComponent<Text>().text = currentNumber + number;
            }
        }
    }

    public void NumberInputClear()
    {
        GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel/NumberEnteredPanel/NumberEnteredText").GetComponent<Text>().text = "0";
    }

    public void NumberInputConfirm()
    {
        GameManager.instance.numberInput = int.Parse(GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel/NumberEnteredPanel/NumberEnteredText").GetComponent<Text>().text);
    }
}
