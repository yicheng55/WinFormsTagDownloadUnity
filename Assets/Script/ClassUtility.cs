using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;     //for MessageBox
using UnityEngine;
using System.Diagnostics;

namespace WinFormsTagDownload
{
    public class TClassUtility
    {
        //private Form Owner;
        static int RunCount = 10000;

        //建構子
        //public TClassUtility(Form Owner)
        //{
        //    //會跟著釋放
        //    this.Owner = Owner;
        //    if (Owner == null)
        //    {
        //        Owner = new Form();
        //    }
        //}



        static void ListTest()
        {
            List<byte> byteSource = new List<byte>();
            byteSource.Add(11);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < RunCount; i++)
            {
                byte[] newData = new byte[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                byteSource.AddRange(newData);
            }
            byte[] data = byteSource.ToArray();
            //byte[] subData = byteSource.Take(100).ToArray();//獲取前100個位元組
            //byteSource.RemoveRange(0, 100);//取出後刪除
            //byteSource.GetRange(100, 100);//從下標100開始取100個位元組
            sw.Stop();
            Console.WriteLine("ListTest " + sw.ElapsedMilliseconds + " 毫秒,陣列長度：" + data.Length);
        }

        static void ArrayCopyTest()
        {
            byte[] byteSource = new byte[1] { 11 };
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < RunCount; i++)
            {
                byte[] newData = new byte[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                byte[] tmp = new byte[byteSource.Length + newData.Length];
                Array.Copy(byteSource, tmp, byteSource.Length);
                Array.Copy(newData, 0, tmp, byteSource.Length, newData.Length);
                byteSource = tmp;
            }
            sw.Stop();
            Console.WriteLine("ArrayCopyTest " + sw.ElapsedMilliseconds + " 毫秒,陣列長度：" + byteSource.Length);
        }

        static void BlockCopyTest()
        {
            byte[] byteSource = new byte[1] { 11 };
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < RunCount; i++)
            {
                byte[] newData = new byte[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                byte[] tmp = new byte[byteSource.Length + newData.Length];
                System.Buffer.BlockCopy(byteSource, 0, tmp, 0, byteSource.Length);
                System.Buffer.BlockCopy(newData, 0, tmp, byteSource.Length, newData.Length);
                byteSource = tmp;
            }
            sw.Stop();
            Console.WriteLine("BlockCopyTest " + sw.ElapsedMilliseconds + " 毫秒,陣列長度：" + byteSource.Length);
        }


        public void ArrayCopyTestFunction()
        {
            ArrayCopyTest();
            BlockCopyTest();
            ListTest();
            Console.ReadKey();
        }

        public void DumpBytes(byte[] bdata, int len)
        {
            int i;
            int j = 0;
            char dchar;

            // 3 * 16 chars for hex display, 16 chars for text and 8 chars
            // for the 'gutter' int the middle.
            StringBuilder dumptext = new StringBuilder("        ", 16 * 4 + 8);
            for (i = 0; i < len; i++)
            {
                dumptext.Insert(j * 3, String.Format("{0:X2} ", (int)bdata[i]));
                dchar = (char)bdata[i];
                //' replace 'non-printable' chars with a '.'.
                if (Char.IsWhiteSpace(dchar) || Char.IsControl(dchar))
                {
                    dchar = '.';
                }
                dumptext.Append(dchar);
                j++;
                if (j == 16)
                {
                    Console.WriteLine(dumptext);
                    dumptext.Length = 0;
                    dumptext.Append("        ");
                    j = 0;
                }
            }
            // display the remaining line
            if (j > 0)
            {
                for (i = j; i < 16; i++)
                {
                    dumptext.Insert(j * 3, "   ");
                }
                Console.WriteLine(dumptext);
            }
        }

        public string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder str = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    str.Append(bytes[i].ToString("X2"));
                }
                hexString = str.ToString();
            }
            return hexString;
        }



#if true

        /// <summary>
        /// Hex String 轉 byte[]
        /// </summary>
        /// <param name="newString">Hex String</param>
        /// <returns>byte[]</returns>
        public byte[] GetBytes(string HexString)
        {
            int byteLength = HexString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { HexString[j], HexString[j + 1] });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }

        public byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }

#endif


        public static byte[] HexToByteArray(string hexString)
        {
            //運算後的位元組長度:16進位數字字串長/2
            byte[] byteOUT = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i = i + 2)
            {
                //每2位16進位數字轉換為一個10進位整數
                byteOUT[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return byteOUT;
        }

        public static string ByteArrayToHexString(byte[] Bdata)
        {
            return BitConverter.ToString(Bdata).Replace("-", "");
        }

        public string RunExeByProcess(string exePath, string argument)
        {
            //開啟新執行緒
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            //呼叫的exe的名稱
            process.StartInfo.FileName = exePath;
            //傳遞進exe的引數
            process.StartInfo.Arguments = argument;
            process.StartInfo.UseShellExecute = false;
            //不顯示exe的介面
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

            process.StandardInput.AutoFlush = true;

            string result = null;
            while (!process.StandardOutput.EndOfStream)
            {
                result += process.StandardOutput.ReadLine() + Environment.NewLine;
            }
            process.WaitForExit();
            return result;
        }

        /////////*****************************************************
        public int ExecuteCommand(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;
            //MessageBoxButtons buttons = MessageBoxButtons.OK;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            string output = process.StandardOutput.ReadToEnd();   //要先輸出.若在畫面輸出的資料量大的時候，Powershell畫面所輸出的資料是會將Buffer寫滿的
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            //string output = process.StandardOutput.ReadToEnd();   //若在畫面輸出的資料量大的時候，Powershell畫面所輸出的資料是會將Buffer寫滿的
            string error = process.StandardError.ReadToEnd();


            exitCode = process.ExitCode;
            if (output.Contains("Reset type NORMAL:"))
            {
                Console.WriteLine("Can connect to target..........");

            }
            else
            {
                exitCode = 2;
                error = "Fail....";
                Console.WriteLine("Cannot connect to target....");
            }

            //if (output.Contains("O.K"))
            //{

            //    Console.WriteLine("Download Flash OK....");
            //}
            //else
            //{
            //    exitCode = 2;
            //    Console.WriteLine("Download Flash Fail..........");
            //}

            string message = String.Format("error = {0} \r\nexitCode = {1}", (String.IsNullOrEmpty(error) ? "(none)" : error), exitCode);
            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            //MessageBox.Show(message, "Show...", buttons);
            process.Close();
            return exitCode;
        }


        public int ExecuteCommandTest(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;
            //MessageBoxButtons buttons = MessageBoxButtons.OK;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            string output = process.StandardOutput.ReadToEnd();   //要先輸出.若在畫面輸出的資料量大的時候，Powershell畫面所輸出的資料是會將Buffer寫滿的
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            //string output = process.StandardOutput.ReadToEnd();   //若在畫面輸出的資料量大的時候，Powershell畫面所輸出的資料是會將Buffer寫滿的
            string error = process.StandardError.ReadToEnd();

            //bool containsSearchResult = output.Contains("WinFormsTagDownload");
            ////bool containsSearchResult = output.Contains("WinFormsTagDownload");
            //Console.WriteLine($"Contains \"WinFormsTagDownload\"? {containsSearchResult}");

            exitCode = process.ExitCode;
            if (output.Contains("Reset type NORMAL:"))
            {
                Console.WriteLine("Can connect to target..........");

            }
            else
            {
                exitCode = 2;
                error = "Fail....";
                string message = String.Format("error = {0} \r\nexitCode = {1}", (String.IsNullOrEmpty(error) ? "(none)" : error), exitCode);
                Console.WriteLine("Cannot connect to target....");
                //MessageBox.Show(message, "Show...", buttons);
            }

            //string message = String.Format("error = {0} \r\nexitCode = {1}", (String.IsNullOrEmpty(error) ? "(none)" : error), exitCode);
            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            //MessageBox.Show(message, "Show...", buttons);
            process.Close();

            return exitCode;
        }

        /*
        static void Main()
        {
            ExecuteCommand("echo testing");
        }
        */



    }
}
