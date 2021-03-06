using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
//using System.Windows.Forms;
using WinFormsTagDownload;
using EasyUI.Dialogs;
using UnityEngine.EventSystems;
using System.Windows.Forms;

public class Download : MonoBehaviour
{
    public InputField   textTest;
    public InputField[] tbx_id;
    public InputField   tbx_TempRef;
    public InputField tbx_LSMin;
    public InputField tbx_LSHour;
    public InputField tbx_RfChannel;

    public InputField inTempCnt;
    public InputField inRfPower;
    public InputField inBatteryDetCnt;
    public InputField inHighDrivingCurrent;

    public Dropdown mDropWakeUpSec;
    public Dropdown mDropRptCnt;
    public Dropdown mDropMaxCnt;


    public GameObject dialogUI;


    int score, cnt;
    int mDropWakeUpSecValue;


    private byte[] readBin;
    private const int TEMPERATURE_NVM_BASE_ADDR = 0x3EE80;
    private const int NVM_BASE_START_ADDR = 0x3EF00;
    private const int ACTIVE_NVM_BASE_ADDR = 0x3EF30;

    private const int NVM_BASE_TAGID0_ADDR = 0x3EF02;
    private const int NVM_BASE_TAGID1_ADDR = 0x3EF03;
    private const int NVM_BASE_TAGID2_ADDR = 0x3EF04;
    private const int NVM_BASE_TAGID3_ADDR = 0x3EF05;
    private const int NVM_BASE_TAGID4_ADDR = 0x3EF06;
    //gDatarate      = 0x3EF07;
    private const int NVM_BASE_RfChn_ADDR = 0x3EF08;
    private const int NVM_BASE_WakeUp_ADDR = 0x3EF09;
    private const int NVM_BASE_LSMin_ADDR = 0x3EF0A;
    private const int NVM_BASE_LSHour_ADDR = 0x3EF0B;
    //DefaultRepeaterID[5]  = 0x3EF0C;
    //MaxTagID[5] = 0x3EF11;
    //NVM_version = 0x3EF16;
    private const int NVM_BASE_WakeUpTempCnt_ADDR = 0x3EF17;
    private const int NVM_BASE_HourCheckBatteryDetect0_ADDR = 0x3EF18;
    private const int NVM_BASE_HourCheckBatteryDetect1_ADDR = 0x3EF19;
    //EPD_Size       = 0x3EF1A;
    //EPD_Color      = 0x3EF1B;
    //Option
    //AutoReport
    //CheckSum
    private const int NVM_BASE_MaxRetryCount_ADDR = 0x3EF1F;
    private const int NVM_BASE_AdcBase_ADDR = 0x3EF20;
    private const int NVM_BASE_Reserved_ADDR = 0x3EF22;

    private const int NVM_BASE_ReportCnt_ADDR = 0x3EF32;
    //DevType
    private const int NVM_BASE_RfPower_ADDR = 0x3EF34;

    ////private const int NVM_BASE_RfChn_ADDR = 0x3EF08;
    ////private const int NVM_BASE_WakeUp_ADDR = 0x3EF09;
    ////private const int NVM_BASE_LSMin_ADDR = 0x3EF0A;
    ////private const int NVM_BASE_LSHour_ADDR = 0x3EF0B;
    ////private const int NVM_BASE_MaxRetryCount_ADDR = 0x3EF1F;
    ////private const int NVM_BASE_AdcBase_ADDR = 0x3EF20;
    ////private const int NVM_BASE_Reserved_ADDR = 0x3EF22;

    ////private const int NVM_BASE_ReportCnt_ADDR = 0x3EF32;
    private const int NVM_BASE_TempRef_ADDRL = 0x3EE86;
    private const int NVM_BASE_TempRef_ADDRH = 0x3EE87;

    private TClassUtility ClassUtility;

    private List<GameObject> inputList;
    EventSystem system;

    // Start is called before the first frame update
    void Start()
    {
        ClassUtility = new TClassUtility();
        Debug.Log("Start()!!!!!!");
        textTest.text = "JLinkWrite.bat";
        tbx_id[0].characterLimit = 2;
        tbx_id[1].characterLimit = 2;
        tbx_id[2].characterLimit = 2;
        tbx_id[3].characterLimit = 2;
        tbx_id[4].characterLimit = 2;

        tbx_LSMin.characterLimit = 2;
        tbx_LSHour.characterLimit = 2;
        tbx_RfChannel.characterLimit = 3;

        //string strCmdText;
        //strCmdText = " /C JLinkConnectTest.bat";   //This command to open a new notepad
        //System.Diagnostics.Process.Start("CMD.exe", strCmdText);    //Start cmd process

        //strCmdText = "/C notepad";   //This command to open a new notepad
        //System.Diagnostics.Process.Start("CMD.exe", strCmdText);    //Start cmd process

        BinaryFileRead();


        system = EventSystem.current;
        inputList = new List<GameObject>();
        InputField[] array = transform.GetComponentsInChildren<InputField>();
        Debug.Log("array.Length= " + array.Length);

        for (int i = 0; i < array.Length; i++)
        {
            inputList.Add(array[i].gameObject);
            Debug.Log("array= " + i);
        }

        //system = EventSystem.current;
        //inputList = new List<GameObject>();
        //tbx_id = transform.GetComponentsInChildren<InputField>();
        //for (int i = 0; i < tbx_id.Length; i++)
        //{
        //    inputList.Add(tbx_id[i].gameObject);
        //    Debug.Log("tbx_id= " + i);
        //}
    }

    void Update()
    {
        //Detect when the up arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Debug.Log("Up Arrow key was pressed.");

        //Detect when the up arrow key has been released
        if (Input.GetKeyUp(KeyCode.UpArrow))
            Debug.Log("Up Arrow key was released.");

        //Detect when the up arrow key has been released
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Debug.Log("Tab key was released.");
            tbx_id4_TextChanged();
        }

        //if (Input.GetKeyDown(KeyCode.Return) && tbx_id[4].text == "Hands")
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter key was released.");
            tbx_id4_TextChanged();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputList.Contains(system.currentSelectedGameObject))
            {
                //Positive order
                GameObject next = NextInput(system.currentSelectedGameObject);
                system.SetSelectedGameObject(next);
                ////Reverse order
                //GameObject last = LastInput(system.currentSelectedGameObject);
                //system.SetSelectedGameObject(last);
            }
        }

        //Debug.Log("Update()!!!!!!... " + cnt++);
    }


    //Get the last object
    private GameObject LastInput(GameObject input)
    {
        int indexNow = IndexNow(input);
        if (indexNow - 1 >= 0)
        {
            return inputList[indexNow - 1].gameObject;
        }
        else
        {
            return inputList[inputList.Count - 1].gameObject;
        }
    }
    //Get the sequence of the currently selected object
    private int IndexNow(GameObject input)
    {
        int indexNow = 0;
        for (int i = 0; i < inputList.Count; i++)
        {
            if (input == inputList[i])
            {
                indexNow = i;
                break;
            }
        }
        return indexNow;
    }

    //Get the next object
    private GameObject NextInput(GameObject input)
    {
        int indexNow = IndexNow(input);
        if (indexNow + 1 < inputList.Count)
        {
            return inputList[indexNow + 1].gameObject;
        }
        else
        {
            return inputList[0].gameObject;
        }
    }


    public void ExitButton()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene(0);

    }
    public void ReturnButton()
    {
        dialogUI.SetActive(true);
        DialogUI.Instance
        .SetTitle("Message 1")
        .SetMessage("Hello James!")
        .SetButtonColor(DialogButtonColor.Blue)
        .OnClose(() => Debug.Log("Closed 1"))
        .Show();
    }


    public void btn_tabP1_Burn1_Click()
    {
        int retcode;

        try
        {

            //retcode = ClassUtility.ExecuteCommandTest("dir");
            retcode = ClassUtility.ExecuteCommandTest("JLinkConnectTest.bat");
            if (retcode != 0)
            {
                Debug.Log("Down load Fail.... ");
                //toolStripStatusLabel1.Text = "Down load Fail......";
                //statusStrip1.Update();
                dialogUI.SetActive(true);
                DialogUI.Instance
                    .SetTitle("Message 1")
                    .SetMessage("JLinkConnectTest Fail111.... JLinkConnectTest.bat")
                    .SetButtonColor(DialogButtonColor.Blue)
                    .OnClose(() => Debug.Log("Closed 1"))
                    .Show();

            }
            else
            {
                //ClassUtility.ExecuteCommand("dir");
                ClassUtility.ExecuteCommand(textTest.text);
                //toolStripStatusLabel1.Text = "Down load success......";
                //statusStrip1.Update();
                Debug.Log("Down load success.... JLinkConnectTest.bat");
            }

        }
        catch (IOException e)
        {
            //MessageBoxButtons buttons = MessageBoxButtons.OK;
            Debug.Log("\nException Caught!");
            //Debug.Log("Message :{0} ", e.Message);
            Debug.Log(e.Message + " Cannot read from file.");
            //EditorUtility.DisplayDialog("DialogMsg", e.Message, "Yes");
            dialogUI.SetActive(true);
            DialogUI.Instance
                .SetTitle("Message 1")
                .SetMessage(e.Message)
                .SetButtonColor(DialogButtonColor.Blue)
                .OnClose(() => Debug.Log("Closed 1"))
                .Show();

            return;
        }

    }



    public void btn_tabP1_Burn2_Click()
    {
        int retcode;
        retcode = ClassUtility.ExecuteCommandTest("JLinkConnectTest.bat");
        if (retcode != 0)
        {

            Debug.Log("JLinkConnectTest Fail.... ");
            //toolStripStatusLabel1.Text = "Down load Fail......";
            //statusStrip1.Update();

            dialogUI.SetActive(true);
            DialogUI.Instance
                .SetTitle("Message 1")
                .SetMessage("JLinkConnectTest Fail222.... ")
                .SetButtonColor(DialogButtonColor.Blue)
                .OnClose(() => Debug.Log("Closed 1"))
                .Show();

            return;
        }

        byte[] TAG_ID = new byte[5];
        TAG_ID[1] = ClassUtility.HexToByte(tbx_id[1].text);      //tbx_id4.Text
        TAG_ID[2] = ClassUtility.HexToByte(tbx_id[2].text);      //tbx_id4.Text
        TAG_ID[3] = ClassUtility.HexToByte(tbx_id[3].text);      //tbx_id4.Text
        TAG_ID[4] = ClassUtility.HexToByte(tbx_id[4].text);      //tbx_id4.Text

        TAG_ID[4] = (byte)(TAG_ID[4] + 1);

        if (((TAG_ID[4]) & 0xFF) == 0xFF)
        {
            TAG_ID[4] = 0x01;

            if (TAG_ID[3] < 0xFF)
            {
                TAG_ID[3] = (byte)(TAG_ID[3] + 1);
                tbx_id[3].text = TAG_ID[3].ToString("X2");
            }
            else if (TAG_ID[3] == 0xFF)
            {
                TAG_ID[3] = 0x00;
                TAG_ID[2] = (byte)(TAG_ID[2] + 1);
                tbx_id[3].text = TAG_ID[3].ToString("X2");
                tbx_id[2].text = TAG_ID[2].ToString("X2");

            }
        }
        tbx_id[4].text = TAG_ID[4].ToString("X2");

        TAG_ID[0] = (byte)((TAG_ID[1] + TAG_ID[2] + TAG_ID[3] + ((TAG_ID[4]) & 0xFF)) % 255);
        tbx_id[0].text = TAG_ID[0].ToString("X2");
        Debug.Log("TAG_ID:" + TAG_ID[0].ToString("X2") + TAG_ID[1].ToString("X2") + TAG_ID[2].ToString("X2") + TAG_ID[3].ToString("X2") + TAG_ID[4].ToString("X2"));

        btn_tabP1_SaveFile_Click();

        btn_tabP1_Burn1_Click();

    }


    public void TestButton()
    {
        //string m_Message;
        Debug.Log("textTest: " + textTest.text);

        //textTest.text = score.ToString();
        //Debug.Log(score);

        mDropWakeUpSecValue = mDropWakeUpSec.value;
        Debug.Log("WakeUpSec = " + mDropWakeUpSecValue);

        //m_Message = mDropWakeUpSec.options[score].text;
        //Debug.Log("m_Message = " + m_Message);

        //mDropWakeUpSec.captionText.text = mDropWakeUpSec.options[score].text;
        Debug.Log("mDropWakeUpSec = " + mDropWakeUpSec.captionText.text);
        Debug.Log("mDropRptCnt = " + mDropRptCnt.captionText.text);
        Debug.Log("mDropMaxCnt = " + mDropMaxCnt.captionText.text);

        score++;
        tbx_id4_TextChanged();
        btn_tabP1_SaveFile_Click();
    }


    public void tbx_id4_TextChanged()
    {
        //String valueOne = this.tbx_id4.Text;
        //int tmp;
        byte[] TAG_ID = new byte[5];
        TAG_ID[1] = ClassUtility.HexToByte(tbx_id[1].text);      //tbx_id4.Text
        TAG_ID[2] = ClassUtility.HexToByte(tbx_id[2].text);      //tbx_id4.Text
        TAG_ID[3] = ClassUtility.HexToByte(tbx_id[3].text);      //tbx_id4.Text
        TAG_ID[4] = ClassUtility.HexToByte(tbx_id[4].text);      //tbx_id4.Text
        TAG_ID[0] = (byte)((TAG_ID[1] + TAG_ID[2] + TAG_ID[3] + ((TAG_ID[4]) & 0xFF)) % 255);
        tbx_id[0].text = TAG_ID[0].ToString("X2");

        if ((TAG_ID[4] == 0xFF) || TAG_ID[4] == 0x00)
        {
            dialogUI.SetActive(true);
            DialogUI.Instance
                .SetTitle("Message warning!!!")
                .SetMessage("Input TAG_ID warning!!! ")
                .SetButtonColor(DialogButtonColor.Blue)
                .OnClose(() => Debug.Log("Closed 1"))
                .Show();
            //MessageBoxButtons buttons = MessageBoxButtons.OK;
            //MessageBox.Show(" Input TAG_ID warning!!! ", "Warning...", buttons);
        }


        //TAG_ID[0] = ClassUtility.HexToByte(tbx_id0.Text);      //tbx_id4.Text
        //ByteArray[0] = ClassUtility.HexToByte(valueOne);      //tbx_id4.Text
        /*
                    readBin[NVM_BASE_TAGID0_ADDR] = TAG_ID[0];
                    readBin[NVM_BASE_TAGID1_ADDR] = TAG_ID[1];
                    readBin[NVM_BASE_TAGID2_ADDR] = TAG_ID[2];
                    readBin[NVM_BASE_TAGID3_ADDR] = TAG_ID[3];
                    readBin[NVM_BASE_TAGID4_ADDR] = TAG_ID[4];
        */
        Debug.Log("Sum: " + TAG_ID[0]);
    }

    private void button4_Click(object sender, EventArgs e)
    {
        int retcode;
        //ClassUtility.ExecuteCommand(textBox1.Text);
        Console.WriteLine("ClassUtility.ExecuteCommandTest(JLinkConnectTest.bat);");
        //ClassUtility.ExecuteCommandTest(textBox1.Text);             //JLinkConnectTest.bat
        retcode = ClassUtility.ExecuteCommandTest("JLinkConnectTest.bat");             //JLinkConnectTest.bat
        if (retcode != 0)
        {
            //statusStrip1.Text = "Down load Fail.... ";
            //toolStripStatusLabel1.Text = "Down load Fail......";
            //statusStrip1.Update();
            Debug.Log("Down load Fail.... ");
            //return;

        }
        else
        {
            //statusStrip1.Text = "Down load success.... ";
            //toolStripStatusLabel1.Text = "Down load success......";
            //statusStrip1.Update();
            Debug.Log("Down load success.... ");
        }
    }


    private void BinaryFileRead()
    {
        //byte[] ByteArray = { 84};
        //byte[] ByteArray = { 84, 104, 105, 115, 32, 105, 115, 32, 101, 120, 97, 109, 112, 108, 101, };
        byte[] ByteArray = new byte[1];
        //byte[] ByteArray2 = new byte[2];
        short TempRef;
        double dTempRef;
        List<byte> byteSource = new List<byte>();

        try
        {
            Debug.Log("BinaryFileRead()");
            string path = @"hex.bin";
            // Calling the ReadAllBytes() function
            readBin = File.ReadAllBytes(path);
            byteSource.AddRange(readBin);
            //Debug.Log("byteSource len:      {0}", byteSource.Count.ToString("X8"));

            //Console.WriteLine("byteSource List<byte>: {0}", byteSource[NVM_BASE_TAGID0_ADDR].ToString("X2"));

            /*
            Buffer.BlockCopy(readBin, NVM_BASE_TAGID0_ADDR, ByteArray, 0, 1);
            tbx_id0.Text = ClassUtility.ToHexString(ByteArray);

            Buffer.BlockCopy(readBin, NVM_BASE_TAGID1_ADDR, ByteArray, 0, 1);
            tbx_id1.Text = ClassUtility.ToHexString(ByteArray);

            Buffer.BlockCopy(readBin, NVM_BASE_TAGID2_ADDR, ByteArray, 0, 1);
            tbx_id2.Text = ClassUtility.ToHexString(ByteArray);

            Buffer.BlockCopy(readBin, NVM_BASE_TAGID3_ADDR, ByteArray, 0, 1);
            tbx_id3.Text = ClassUtility.ToHexString(ByteArray);

            Buffer.BlockCopy(readBin, NVM_BASE_TAGID4_ADDR, ByteArray, 0, 1);
            tbx_id4.Text = ClassUtility.ToHexString(ByteArray);
            */

            tbx_id[0].text = readBin[NVM_BASE_TAGID0_ADDR].ToString("X2");
            tbx_id[1].text = readBin[NVM_BASE_TAGID1_ADDR].ToString("X2");
            tbx_id[2].text = readBin[NVM_BASE_TAGID2_ADDR].ToString("X2");
            tbx_id[3].text = readBin[NVM_BASE_TAGID3_ADDR].ToString("X2");
            tbx_id[4].text = readBin[NVM_BASE_TAGID4_ADDR].ToString("X2");

            tbx_RfChannel.text = readBin[NVM_BASE_RfChn_ADDR].ToString();
            //tbx_WakeUp.Text = readBin[NVM_BASE_WakeUp_ADDR].ToString();
            //comboBox1.Text = readBin[NVM_BASE_WakeUp_ADDR].ToString();

            tbx_LSMin.text = readBin[NVM_BASE_LSMin_ADDR].ToString();
            tbx_LSHour.text = readBin[NVM_BASE_LSHour_ADDR].ToString();

            //tbx_RpCnt.Text = readBin[NVM_BASE_ReportCnt_ADDR].ToString();
            //cbx_ReportCnt.Text = readBin[NVM_BASE_ReportCnt_ADDR].ToString();
            //cbx_RetryCnt.Text = readBin[NVM_BASE_MaxRetryCount_ADDR].ToString();

            //Buffer.BlockCopy(readBin, NVM_BASE_TempRef_ADDR, ByteArray2, 0, 2);

            mDropWakeUpSec.captionText.text = readBin[NVM_BASE_WakeUp_ADDR].ToString();
            mDropRptCnt.captionText.text = readBin[NVM_BASE_ReportCnt_ADDR].ToString();

            mDropMaxCnt.captionText.text = readBin[NVM_BASE_MaxRetryCount_ADDR].ToString();
 
            Debug.Log("mDropWakeUpSec.captionText.text =" + mDropWakeUpSec.captionText.text);
            Debug.Log("mDropRptCnt.captionText.text =" + mDropRptCnt.captionText.text);

            Debug.Log("mDropMaxCnt.captionText.text =" + mDropMaxCnt.captionText.text);
            Debug.Log("readBin[NVM_BASE_MaxRetryCount_ADDR] =" + readBin[NVM_BASE_MaxRetryCount_ADDR].ToString());

            TempRef = (short)(readBin[NVM_BASE_TempRef_ADDRH] << 8 | readBin[NVM_BASE_TempRef_ADDRL]);
            dTempRef = TempRef / 10.0;
            string message = String.Format("TempRef = {0}", dTempRef);
            Debug.Log(message);
            Debug.Log("AAAA___TempRef =" + dTempRef );
            tbx_TempRef.text = dTempRef.ToString("0.0");

            /*             Buffer.BlockCopy(readBin, NVM_BASE_TempRef_ADDR, ByteArray2, 0, 2);
                         TempRef = (ushort)(ByteArray2[1] << 8 | ByteArray2[0]);
                         dTempRef = TempRef / 10.0;
                         Console.WriteLine("TempRef:      {0}", dTempRef);
                         tbx_TempRef.Text = dTempRef.ToString();    */

            //ClassUtility.DumpBytes(ByteArray, ByteArray.Length);

            inTempCnt.text = readBin[NVM_BASE_WakeUpTempCnt_ADDR].ToString();
            inRfPower.text = readBin[NVM_BASE_RfPower_ADDR].ToString("X2");
            inBatteryDetCnt.text = readBin[NVM_BASE_HourCheckBatteryDetect0_ADDR].ToString();
            inHighDrivingCurrent.text = readBin[NVM_BASE_HourCheckBatteryDetect1_ADDR].ToString();

            Debug.Log("NVM_BASE_WakeUpTempCnt:"+ readBin[NVM_BASE_WakeUpTempCnt_ADDR].ToString());
            Debug.Log("NVM_BASE_HourCheckBatteryDetect0:"+ readBin[NVM_BASE_HourCheckBatteryDetect0_ADDR].ToString());
            Debug.Log("NVM_BASE_HourCheckBatteryDetect1:"+ readBin[NVM_BASE_HourCheckBatteryDetect1_ADDR].ToString());
            Debug.Log("NVM_BASE_RfPower:0x"+readBin[NVM_BASE_RfPower_ADDR].ToString("X2"));

            Debug.Log("BinaryFileRead() ... end");
        }
        catch (IOException e)
        {
            //MessageBoxButtons buttons = MessageBoxButtons.OK;
            Debug.Log("\nException Caught!");
            //Debug.Log("Message :{0} ", e.Message);
            Debug.Log(e.Message + " Cannot read from file.");
            //EditorUtility.DisplayDialog("DialogMsg", e.Message, "Yes");
            dialogUI.SetActive(true);
            DialogUI.Instance
                .SetTitle("Message 1")
                .SetMessage(e.Message)
                .SetButtonColor(DialogButtonColor.Blue)
                .OnClose(() => Debug.Log("Closed 1"))
                .Show();


            return;
        }
    }

    private void BinaryFileWrite()
    {
        try
        {
            Debug.Log("BinaryFileWrite()");
            string path = @"hex.bin";
            // Adding new contents to the file 
            File.WriteAllBytes(path, readBin);
            //ClassUtility.DumpBytes(readBin, readBin.Length);
        }
        catch (IOException e)
        {
            Debug.Log("\nException Caught!");
            Debug.Log("Message : " + e.Message);
            Debug.Log(e.Message + " Cannot write from file.");
            return;
        }
    }


    private void btn_tabP1_SaveFile_Click()
    {
        ushort TempRef;
        double dTempRef;
        //byte[] ByteArray = new byte[30];

        readBin[NVM_BASE_TAGID0_ADDR] = ClassUtility.HexToByte(tbx_id[0].text);      //tbx_id0.Text
        readBin[NVM_BASE_TAGID1_ADDR] = ClassUtility.HexToByte(tbx_id[1].text);      //tbx_id1.Text
        readBin[NVM_BASE_TAGID2_ADDR] = ClassUtility.HexToByte(tbx_id[2].text);      //tbx_id2.Text
        readBin[NVM_BASE_TAGID3_ADDR] = ClassUtility.HexToByte(tbx_id[3].text);      //tbx_id3.Text
        readBin[NVM_BASE_TAGID4_ADDR] = ClassUtility.HexToByte(tbx_id[4].text);      //tbx_id4.Text


        readBin[NVM_BASE_RfChn_ADDR] = Convert.ToByte(tbx_RfChannel.text);

        //readBin[NVM_BASE_WakeUp_ADDR] = Convert.ToByte(tbx_WakeUp.Text);
        readBin[NVM_BASE_WakeUp_ADDR] = Convert.ToByte(mDropWakeUpSec.captionText.text);
        //Console.WriteLine("comboBox1_SelectedString = {0},  {1}", comboBox1.SelectedItem.ToString(), comboBox1.Text);

        readBin[NVM_BASE_LSMin_ADDR] = Convert.ToByte(tbx_LSMin.text);
        readBin[NVM_BASE_LSHour_ADDR] = Convert.ToByte(tbx_LSHour.text);

        readBin[NVM_BASE_ReportCnt_ADDR] = Convert.ToByte(mDropRptCnt.captionText.text);
        //readBin[NVM_BASE_ReportCnt_ADDR] = Convert.ToByte(tbx_RpCnt.Text);
        //readBin[NVM_BASE_Reserved_ADDR] = ClassUtility.HexToByteArray(tbx_Rev.Text);

        readBin[NVM_BASE_MaxRetryCount_ADDR] = Convert.ToByte(mDropMaxCnt.captionText.text);

        readBin[NVM_BASE_WakeUpTempCnt_ADDR] = Convert.ToByte(inTempCnt.text);
        readBin[NVM_BASE_RfPower_ADDR] = ClassUtility.HexToByte(inRfPower.text);
        readBin[NVM_BASE_HourCheckBatteryDetect0_ADDR] = Convert.ToByte(inBatteryDetCnt.text);
        readBin[NVM_BASE_HourCheckBatteryDetect1_ADDR] = Convert.ToByte(inHighDrivingCurrent.text);


        dTempRef = Convert.ToDouble(tbx_TempRef.text);
        TempRef = (ushort)(dTempRef * 10);
        readBin[NVM_BASE_TempRef_ADDRL] = (byte)(TempRef & 0x0000FF);
        readBin[NVM_BASE_TempRef_ADDRH] = (byte)((TempRef >> 8) & 0x0000FF);

        Debug.Log("dTempRef = " + dTempRef);
        Debug.Log("TempRef = " + TempRef);
        Debug.Log("TempRef_ADDRL = " + readBin[NVM_BASE_TempRef_ADDRL]);
        Debug.Log("TempRef_ADDRH = " + readBin[NVM_BASE_TempRef_ADDRH]);

        //ByteArray = ClassUtility.HexToByteArray(tbx_Rev.Text);
        //ClassUtility.DumpBytes(ByteArray, ByteArray.Length);

        //Debug.Log("Message :{0} ", ClassUtility.ByteArrayToHexString(readBin[NVM_BASE_ReportCnt_ADDR]);
        Debug.Log("button1_Click()..................");
        BinaryFileWrite();

        //byte[] byteArray = System.Text.Encoding.Default.GetBytes(tbx_Rev.Text);
        //Console.WriteLine("byteArray.Length: {0}", byteArray.Length);

        //System.Text.Encoding.Default.GetBytes(String); //string轉byte[]
        //System.Text.Encoding.Default.GetString(byte[]);//byte[]轉String 

        //ClassUtility.ArrayCopyTestFunction();


    }

    public void btn_tabP1_OpenFileButton()
    {
        string filename;
        short TempRef;
        double dTempRef;
        List<byte> byteSource = new List<byte>();

        //MessageBoxButtons buttons = MessageBoxButtons.OK;
        //MessageBox.Show(" Input TAG_ID warning!!! ", "Warning...    ", buttons);   //MessageBox.Show Test OK.

        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "exe files (*.bin)|*.bin";  //过滤文件类型  
        dialog.InitialDirectory = ".";  //定义打开的默认文件夹位置，可以在显示对话框之前设置好各种属性  
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            filename = dialog.FileName;
            Debug.Log(filename);


            try
            {
                Debug.Log("BinaryFileRead()");
                string path = @"hex.bin";
                // Calling the ReadAllBytes() function
                ////readBin = File.ReadAllBytes(path);
                readBin = File.ReadAllBytes(filename);
                byteSource.AddRange(readBin);


                tbx_id[0].text = readBin[NVM_BASE_TAGID0_ADDR].ToString("X2");
                tbx_id[1].text = readBin[NVM_BASE_TAGID1_ADDR].ToString("X2");
                tbx_id[2].text = readBin[NVM_BASE_TAGID2_ADDR].ToString("X2");
                tbx_id[3].text = readBin[NVM_BASE_TAGID3_ADDR].ToString("X2");
                tbx_id[4].text = readBin[NVM_BASE_TAGID4_ADDR].ToString("X2");

                tbx_RfChannel.text = readBin[NVM_BASE_RfChn_ADDR].ToString();


                tbx_LSMin.text = readBin[NVM_BASE_LSMin_ADDR].ToString();
                tbx_LSHour.text = readBin[NVM_BASE_LSHour_ADDR].ToString();


                mDropWakeUpSec.captionText.text = readBin[NVM_BASE_WakeUp_ADDR].ToString();
                mDropRptCnt.captionText.text = readBin[NVM_BASE_ReportCnt_ADDR].ToString();

                mDropMaxCnt.captionText.text = readBin[NVM_BASE_MaxRetryCount_ADDR].ToString();

                Debug.Log("mDropWakeUpSec.captionText.text =" + mDropWakeUpSec.captionText.text);
                Debug.Log("mDropRptCnt.captionText.text =" + mDropRptCnt.captionText.text);

                Debug.Log("mDropMaxCnt.captionText.text =" + mDropMaxCnt.captionText.text);
                Debug.Log("readBin[NVM_BASE_MaxRetryCount_ADDR] =" + readBin[NVM_BASE_MaxRetryCount_ADDR].ToString());

                TempRef = (short)(readBin[NVM_BASE_TempRef_ADDRH] << 8 | readBin[NVM_BASE_TempRef_ADDRL]);
                dTempRef = TempRef / 10.0;
                string message = String.Format("TempRef = {0}", dTempRef);
                Debug.Log(message);
                Debug.Log("AAAA___TempRef =" + dTempRef);
                tbx_TempRef.text = dTempRef.ToString("0.0");


                inTempCnt.text = readBin[NVM_BASE_WakeUpTempCnt_ADDR].ToString();
                inRfPower.text = readBin[NVM_BASE_RfPower_ADDR].ToString("X2");
                inBatteryDetCnt.text = readBin[NVM_BASE_HourCheckBatteryDetect0_ADDR].ToString();
                inHighDrivingCurrent.text = readBin[NVM_BASE_HourCheckBatteryDetect1_ADDR].ToString();

                Debug.Log("NVM_BASE_WakeUpTempCnt:" + readBin[NVM_BASE_WakeUpTempCnt_ADDR].ToString());
                Debug.Log("NVM_BASE_HourCheckBatteryDetect0:" + readBin[NVM_BASE_HourCheckBatteryDetect0_ADDR].ToString());
                Debug.Log("NVM_BASE_HourCheckBatteryDetect1:" + readBin[NVM_BASE_HourCheckBatteryDetect1_ADDR].ToString());
                Debug.Log("NVM_BASE_RfPower:0x" + readBin[NVM_BASE_RfPower_ADDR].ToString("X2"));

                Debug.Log("BinaryFileRead() ... end");
            }
            catch (IOException e)
            {
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                Debug.Log("\nException Caught!");
                //Debug.Log("Message :{0} ", e.Message);
                Debug.Log(e.Message + " Cannot read from file.");
                //EditorUtility.DisplayDialog("DialogMsg", e.Message, "Yes");
                dialogUI.SetActive(true);
                DialogUI.Instance
                    .SetTitle("Message 1")
                    .SetMessage(e.Message)
                    .SetButtonColor(DialogButtonColor.Blue)
                    .OnClose(() => Debug.Log("Closed 1"))
                    .Show();


                return;
            }

        }

    }
}
