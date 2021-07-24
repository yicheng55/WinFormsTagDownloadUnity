using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using WinFormsTagDownload;


public class Download : MonoBehaviour
{
    public InputField   textTest;
    public InputField[] tbx_id;
    public InputField   tbx_TempRef;
    public InputField tbx_LSMin;
    public InputField tbx_LSHour;
    public InputField tbx_RfChannel;

    public Dropdown mDropWakeUpSec;
    public Dropdown mDropRptCnt;
    public Dropdown mDropMaxCnt;


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

    private const int NVM_BASE_RfChn_ADDR = 0x3EF08;
    private const int NVM_BASE_WakeUp_ADDR = 0x3EF09;
    private const int NVM_BASE_LSMin_ADDR = 0x3EF0A;
    private const int NVM_BASE_LSHour_ADDR = 0x3EF0B;
    private const int NVM_BASE_MaxRetryCount_ADDR = 0x3EF1F;
    private const int NVM_BASE_AdcBase_ADDR = 0x3EF20;
    private const int NVM_BASE_Reserved_ADDR = 0x3EF22;

    private const int NVM_BASE_ReportCnt_ADDR = 0x3EF32;
    private const int NVM_BASE_TempRef_ADDRL = 0x3EE86;
    private const int NVM_BASE_TempRef_ADDRH = 0x3EE87;

    private TClassUtility ClassUtility;

    // Start is called before the first frame update
    void Start()
    {
        ClassUtility = new TClassUtility();

        Debug.Log("Start()!!!!!!");
        BinaryFileRead();
    }

    void Update()
    {
        //Debug.Log("Update()!!!!!!... " + cnt++);
    }

    public void ExitButton()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene(0);

    }


    public void btn_tabP1_Burn1_Click()
    {
        int retcode;

        try
        {

            retcode = ClassUtility.ExecuteCommandTest("dir");
            //retcode = ClassUtility.ExecuteCommandTest("JLinkConnectTest.bat");
            if (retcode != 0)
            {
                Debug.Log("Down load Fail.... ");
                //toolStripStatusLabel1.Text = "Down load Fail......";
                //statusStrip1.Update();
            }
            else
            {
                ClassUtility.ExecuteCommand("dir");
                //ClassUtility.ExecuteCommand(textBox1.Text);
                //toolStripStatusLabel1.Text = "Down load success......";
                //statusStrip1.Update();
                Debug.Log("Down load success.... ");
            }

        }
        catch (IOException e)
        {
            //MessageBoxButtons buttons = MessageBoxButtons.OK;
            Debug.Log("\nException Caught!");
            //Debug.Log("Message :{0} ", e.Message);
            Debug.Log(e.Message + " Cannot read from file.");
            EditorUtility.DisplayDialog("DialogMsg", e.Message, "Yes");

            return;
        }

    }


    public void TestButton()
    {
        //string m_Message;
        //Debug.Log(textTest.text);

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
    }

    private void BinaryFileRead()
    {
        //byte[] ByteArray = { 84};
        //byte[] ByteArray = { 84, 104, 105, 115, 32, 105, 115, 32, 101, 120, 97, 109, 112, 108, 101, };
        byte[] ByteArray = new byte[1];
        //byte[] ByteArray2 = new byte[2];
        ushort TempRef;
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

            TempRef = (ushort)(readBin[NVM_BASE_TempRef_ADDRH] << 8 | readBin[NVM_BASE_TempRef_ADDRL]);
            dTempRef = TempRef / 10.0;
            string message = String.Format("TempRef = {0}", dTempRef);
            Debug.Log(message);
            Debug.Log("AAAA___TempRef =" + dTempRef );
            tbx_TempRef.text = dTempRef.ToString("00.0");

            /*             Buffer.BlockCopy(readBin, NVM_BASE_TempRef_ADDR, ByteArray2, 0, 2);
                         TempRef = (ushort)(ByteArray2[1] << 8 | ByteArray2[0]);
                         dTempRef = TempRef / 10.0;
                         Console.WriteLine("TempRef:      {0}", dTempRef);
                         tbx_TempRef.Text = dTempRef.ToString();    */

            //ClassUtility.DumpBytes(ByteArray, ByteArray.Length);
            Debug.Log("BinaryFileRead() ... end");
        }
        catch (IOException e)
        {
            //MessageBoxButtons buttons = MessageBoxButtons.OK;
            Debug.Log("\nException Caught!");
            //Debug.Log("Message :{0} ", e.Message);
            Debug.Log(e.Message + " Cannot read from file.");
            EditorUtility.DisplayDialog("DialogMsg", e.Message, "Yes");

            return;
        }
    }

    void BinaryFileWrite()
    {
        try
        {
            Console.WriteLine("BinaryFileWrite()");
            string path = @"hex.bin";
            // Adding new contents to the file 
            File.WriteAllBytes(path, readBin);
            //ClassUtility.DumpBytes(readBin, readBin.Length);
        }
        catch (IOException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            Console.WriteLine(e.Message + " Cannot write from file.");
            return;
        }
    }

}
