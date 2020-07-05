using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextInput : MonoBehaviour
{
    //Facilitates text font manipulation on text input panels - attached to each individual GameObject for text inputs

    int maxTextInput = 20;
    int maxNameInput = 12;
    int maxNumberInput = 8;

    /// <summary>
    /// Adds given space input or letter to the text input panel
    /// </summary>
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

    /// <summary>
    /// Sets text panel input text to full text input with given letter at the tail
    /// </summary>
    /// <param name="letter">Letter to be added</param>
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

    /// <summary>
    /// Sets GameManager text input to the input text from text input panel
    /// </summary>
    public void TextConfirmClicked()
    {
        if (GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text.Length > 0)
        {
            GameManager.instance.textInput = GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text;
        }
    }

    /// <summary>
    /// Removes letter from the tail of text input panel
    /// </summary>
    public void TextBackspaceClicked()
    {
        string value = GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text;
        if (value.Length > 0)
        {
            value = value.Substring(0, value.Length - 1);
            GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text = value;
        }
    }

    /// <summary>
    /// Sets text input as empty
    /// </summary>
    public void TextClearClicked()
    {
        GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text = "";
    }

    /// <summary>
    /// Starts the coroutine for capitalizing text after clicking shift button
    /// </summary>
    public void TextShiftClicked()
    {
        StartCoroutine(TextShiftCoroutine());
    }

    /// <summary>
    /// Coroutine.  Used after clicking 'shift' button - changes all letters to uppercase (and special characters) for one input.  If caps lock is on, changes all letters to lowercase for one letter.
    /// </summary>
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

    /// <summary>
    /// Switches between upper and lowercase for all letter buttons
    /// </summary>
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

    /// <summary>
    /// Adds given space or letter input to the name input panel
    /// </summary>
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

    /// <summary>
    /// Sets name panel input text to full text input with given letter at the tail
    /// </summary>
    /// <param name="letter">Letter to be added</param>
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

    /// <summary>
    /// Sets GameManager name input to the input text from name input panel
    /// </summary>
    public void NameConfirmClicked()
    {
        if (GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text.Length > 0)
        {
            GameManager.instance.nameInput = GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text;
        }
    }

    /// <summary>
    /// Removes letter from the tail of name input panel
    /// </summary>
    public void NameBackspaceClicked()
    {
        string value = GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text;
        if (value.Length > 0)
        {
            value = value.Substring(0, value.Length - 1);
            GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text = value;
        }
    }

    /// <summary>
    /// Sets name input as empty
    /// </summary>
    public void NameClearClicked()
    {
        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text = "";
    }

    /// <summary>
    /// Starts the coroutine for capitalizing name text after clicking shift button
    /// </summary>
    public void NameShiftClicked()
    {
        StartCoroutine(NameShiftCoroutine());
    }

    /// <summary>
    /// Coroutine.  Used after clicking 'shift' button - changes all letters to uppercase (and special characters) for one input.  If caps lock is on, changes all letters to lowercase for one letter.
    /// </summary>
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

    /// <summary>
    /// Switches between upper and lowercase for all letter buttons
    /// </summary>
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

    /// <summary>
    /// Adds given number input to the number input panel
    /// </summary>
    public void NumberClicked()
    {
        AddNumberInput(gameObject.transform.GetChild(0).GetComponent<Text>().text);
    }

    /// <summary>
    /// Sets number panel input text to full number input with given number at the tail
    /// </summary>
    /// <param name="number">Number to be added</param>
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

    /// <summary>
    /// Sets number text input as empty
    /// </summary>
    public void NumberInputClear()
    {
        GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel/NumberEnteredPanel/NumberEnteredText").GetComponent<Text>().text = "0";
    }

    /// <summary>
    /// Sets GameManager number input to the input text from number input panel
    /// </summary>
    public void NumberInputConfirm()
    {
        GameManager.instance.numberInput = int.Parse(GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel/NumberEnteredPanel/NumberEnteredText").GetComponent<Text>().text);
    }
}
