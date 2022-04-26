using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptLine : MonoBehaviour{
  // general properties
  [SerializeField]
  private int lineNumber = -1;
  private String text;
  private bool hasContent;
  private int indent;

  private bool isSelected;
  private bool highlighted;

  // used when line must overflow onto next
  private GameObject emptyLine;

  // UI text
  private String UIText;

  // vars for keeping track of cursor "|"
  private bool cursorOn;
  private float lastCursorToggleTime;
  private float timeBetweenCursorToggle;

  // gameObject Components
  Text textComponent;

  // constructors
  public ScriptLine(String text, int indent){
    setText(text);
    setIndent(indent);
    if (text != ""){
      setHasContent(true);
    }
    else{
      setHasContent(false);
    }
  }

  public ScriptLine(String text, bool hasContent, int indent){
    setText(text);
    setIndent(indent);
    setHasContent(hasContent);
  }

  // void Start(){
  //   Debug.Log("Start file read:");
  //   string curLine;
  //   System.IO.StreamReader file;
  //   file = new System.IO.StreamReader(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "Script.txt");
  //   while ((curLine = file.ReadLine()) != null)
  //   {
  //     Debug.Log("curLine: " + curLine);
  //   }
  //   file.Close();
  // }

  // Awake is executed before any Start methods
  void Awake(){
    // Debug.Log("Awake file read:");
    // string curLine;
    // System.IO.StreamReader file;
    // file = new System.IO.StreamReader(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "Script.txt");
    // while((curLine = file.ReadLine()) != null)
    // {
    //   Debug.Log("curLine: " + curLine);
    // }
    // file.Close();

    if (lineNumber == 0){
      isSelected = true;
    }else{
      isSelected = false;
    }

    text = "";
    hasContent = false;
    indent = 0;
    cursorOn = false;
    lastCursorToggleTime = 0.0f;
    timeBetweenCursorToggle = 0.5f;

    textComponent = gameObject.transform.Find("Text").GetComponent<Text>();
  }

  // bool hasPrinted = false;
  // Update is called once per frame
  void Update(){
    // if(hasPrinted == false){
    //   hasPrinted = true;
    //   Debug.Log("Update file read:");
    //   string curLine;
    //   System.IO.StreamReader file;
    //   file = new System.IO.StreamReader(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "Script.txt");
    //   while ((curLine = file.ReadLine()) != null)
    //   {
    //     Debug.Log("curLine: " + curLine);
    //   }
    //   file.Close();
    // }

    if (isSelected){
      if (Time.time > lastCursorToggleTime + timeBetweenCursorToggle){
        cursorOn = !cursorOn;
        if (cursorOn){
          textComponent.text = "" + UIText + "|";
        } else{
          textComponent.text = "" + UIText;
        }
        lastCursorToggleTime = Time.time;
      }
    }
  }

  // when user taps on this line
  public void onButtonTap(){
    isSelected = true;
  }

  // get methods
  public int getLineNumber(){
    return lineNumber;
  }

  public String getText(){
    return text;
  }

  public bool getIsSelected(){
    return isSelected;
  }

  public bool getHasContent(){
    return hasContent;
  }

  public int getIndent(){
    return indent;
  }

  public bool getHighlighted(){
    return highlighted;
  }

  // the indentation the line after this should have
  public int getNextLineIndent(){
    int nextIndent = indent;
    if (text.Length > 0 && text[text.Length - 1] == '{'){
      nextIndent++;
    }
    if (nextIndent < 0){
      nextIndent = 0;
    }
    return nextIndent;
  }

  // set methods
  public void setText(String text){
    // set textComponent if it hasn't been set yet
    if(textComponent == null){
      textComponent = gameObject.transform.Find("Text").GetComponent<Text>();
    }
    //Debug.Log("setText 127");
    this.text = text;
    //Debug.Log("setText 129");
    UIText = getUIText();
    //Debug.Log("setText 131");
    //Debug.Log("textComponent = " + textComponent);
    //Debug.Log("UIText = " + UIText);
    textComponent.text = UIText;
    //Debug.Log("setText 133");
  }

  public void setText(String text, int indent){
    this.text = text;
    setIndent(indent);
    UIText = getUIText();
    textComponent.text = UIText;
  }

  public void setHasContent(bool hasContent){
    this.hasContent = hasContent;
    removeEmptyLine();
  }

  public void setHighlighted(bool highlighted){
    this.highlighted = highlighted;
    if (highlighted){
      Color c = gameObject.GetComponent<Image>().color;
      c.a = 100;
      gameObject.GetComponent<Image>().color = c;
    }else{
      Color c = gameObject.GetComponent<Image>().color;
      c.a = 0;
      gameObject.GetComponent<Image>().color = c;
    }
  }

  public void select(){
    this.isSelected = true;
    cursorOn = true;
    textComponent.text = UIText + "|";
    lastCursorToggleTime = Time.time;
  }

  public void deselect(){
    this.isSelected = false;
    cursorOn = false;
    textComponent.text = UIText;
  }

  public void setIndent(int indent){
    this.indent = indent;
    if (text == "}" && indent > 0)
      this.indent--;
    UIText = getUIText();
    textComponent.text = UIText;
  }

  // empty line to add/remove depending if the text fits on one line
  private void insertEmptyLine(){
    if (emptyLine == null){
      emptyLine = (GameObject)Instantiate(Resources.Load("emptyLine"), gameObject.transform.parent);
      emptyLine.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex() + 1);
    }
  }

  private void removeEmptyLine(){
    if (emptyLine != null){
      Destroy(emptyLine);
      emptyLine = null;
    }
  }

  // adds syntax highlighting and indentation to text
  private String getUIText(){
    // "<color=#FFACD5>" + text + "</color>"
    // indent:
    String uiText = "";
    for (int i = 0; i < indent; i++){
      uiText = uiText + "  ";
    }
    // user-defined-function call
    if (text.Length >= 4 && text.Substring(0, 4) == "func" && text.Substring(text.Length - 3, 3) == "();"){
      uiText = uiText + "<color=#ffb89d>" + text.Substring(0, text.Length - 3) + "</color>" + text.Substring(text.Length - 3, 3);
    // API functions (e.g. moveForward();, turnLeft();)
    } else if (text.Length >= 3 && text.Substring(text.Length - 3, 3) == "();"){
      uiText = uiText + "<color=#FFACD5>" + text.Substring(0, text.Length - 3) + "</color>" + text.Substring(text.Length - 3, 3);
    // do loops
    }else if (text.Length >= 2 && text.Substring(0, 2) == "do"){
      uiText = uiText + "<color=#fcea8f>do</color>" + " <color=#83ff89>" + text.Substring(3, 1) + "</color>" + " {";
  	// for loops
    }else if (text.Length >= 3 && text.Substring(0, 3) == "for"){
      uiText = uiText + "<color=#fcea8f>" + text.Substring(0, text.Length - 4) + "</color>" + text.Substring(text.Length - 4, 1)
          + "<color=#83ff89>" + text[text.Length - 3] + "</color>" + text.Substring(text.Length - 2, 2);
    // function declarations
    } else if (text.Length >= 4 && text.Substring(0, 4) == "func" && text[text.Length - 1] == '{'){
      uiText = uiText + "<color=#ffb89d>func" + text.Substring(4, text.Length - 7) + "</color>" + "(){";
    // if statements
    }else if (text.Length >= 2 && text.Substring(0, 2) == "if"){
			// Debug.Log(text.Length + uiText.Length * 2 + 3);
      // if statement does not fit on one line
      if (text.Length + uiText.Length > 23){
        String multilineText = "";
        if (text.Length + uiText.Length * 2 + 3 >= 46){
          multilineText = text.Substring(0, 23 - indent * 2) + '\n' + text.Substring(23 - indent * 2, text.Length - (23 - indent * 2));
          uiText = uiText + "<color=#73D7FF>if</color>" + multilineText.Substring(2, multilineText.Length - 2);
          insertEmptyLine();
        }else{
          for (int i = 24 - uiText.Length; i > 0; i--){
            if (text[i] == ' ' || text[i] == '(' || text[i] == ')'){
              // if the next line will fit with new indent
              if (indent * 2 + text.Length - i + 3 < 22){
                multilineText = text.Substring(0, i + 1) + "\n   ";
                for (int j = 0; j < indent; j++){
                  multilineText = multilineText + "  ";
                }
                multilineText = multilineText + text.Substring(i + 1, text.Length - (i + 1));
              }else{
                multilineText = text.Substring(0, i + 1) + "\n   " + text.Substring(i + 1, text.Length - (i + 1));
              }
              break;
            }
          }
          uiText = uiText + "<color=#73D7FF>if</color>" + multilineText.Substring(2, multilineText.Length - 2);
          insertEmptyLine();
        }
    	// if statement fits on one line
      }else{
        uiText = uiText + "<color=#73D7FF>if</color>" + text.Substring(2, text.Length - 2);
      }

      // IF STATEMENT COLORING

      int openRoundBracket = StringMethods.findFirstOccurance(uiText, '(');
      int dot = StringMethods.findFirstOccurance(uiText, '.');
      int operatorStart = Math.Max(StringMethods.findFirstOccuranceStr(uiText, "!="), 
                                    StringMethods.findFirstOccuranceStr(uiText, "=="));
      int closeRoundBracket = StringMethods.findFirstOccurance(uiText, ')');

      uiText = uiText.Substring(0, openRoundBracket + 1) + "<color=#80B763>" 
              + uiText.Substring(openRoundBracket + 1, dot - openRoundBracket - 1)
              + "</color>" + ".<color=#4581C1>" + uiText.Substring(dot + 1, operatorStart - dot - 2)
              + " </color>" + uiText.Substring(operatorStart, 3) + "<color=#DBDB78>"
              + uiText.Substring(operatorStart + 3, closeRoundBracket - operatorStart - 3) + "</color>"
              + uiText.Substring(closeRoundBracket, uiText.Length - closeRoundBracket);

      // add color to conditional
      /*uiText = uiText.Substring(0, openRoundBracket + 1) + "<color=#80B763>"
                  + uiText.Substring(openRoundBracket + 1, closeRoundBracket - openRoundBracket - 1)
                  + "</color>" + uiText.Substring(closeRoundBracket, uiText.Length - closeRoundBracket);
                  */
      // no highlighting/spacing needing

    }else{
      uiText = uiText + text;
    }
    return uiText;
  }
}