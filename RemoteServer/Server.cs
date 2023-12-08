using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;
using System.Diagnostics;
namespace RemoteServer {
   public class Server
   {
       #region Static members used for our main listen service
       public static Image ImgWinbackground = null;//Our own image with nice white flower on it
       private static bool Closing = false;        //We are shuting down so don't show any error messages 
       private static TcpListener listener = null; //Used to listen for any new client connections, maximium is 3 at the same time
       public static Server Service1 = null;       //Servers for our 3 client connection, only one used most the time
       private static Server Service2 = null;
       private static Server Service3 = null;
       private static Thread THListen = null;      //Our one and only main thread used to wait for new connections
       private static bool FirstConnect = true;       
       private static void ShowDebug(bool Debug)
       {
           Settings.Debug = Debug;
           if (Settings.FormService == null || Closing) return;
           if (Debug)
           {
               Settings.FormService.Invoke((MethodInvoker)delegate()
               {//This code runs on the forms thread
                   Settings.FormService.Show();
                   Settings.FormService.BringToFront();
                   Settings.FormService.Focus();
               });
           }
           else
               Settings.FormService.Invoke((MethodInvoker)delegate()
               {//This code runs on the forms threD
                   Settings.FormService.Hide();
               });
       }

       public static void ListenStart()
       {
           Server.Closing = false;
           THListen = new Thread(new ThreadStart(Listen));
           THListen.Start();
       }

       public static void ListenStop(bool CloseApp)
       {//We don't know what thread this has been called on so we need our own
           Server.Closing = CloseApp;
           Thread TH = new Thread(new ThreadStart(ListenStopNow));
           TH.Start();
       }

       private static void ListenStopNow()
       {//Shut down any services and the listner 
           //Wallpaper.RestoreWallpaper();
           bool CloseApp = Server.Closing;
           Server.Closing = true;
           if (listener != null)
           {
               listener.Server.Close();
               listener.Stop();
           }
           if (Service1 != null && Service1.Running) Service1.Stop();
           if (Service2 != null && Service2.Running) Service2.Stop();
           if (Service3 != null && Service3.Running) Service3.Stop();
           if (THListen != null)
           {
               THListen.Abort();
           }
           if (CloseApp) Environment.Exit(0);
       }

       public static bool IsClientsConnected()
       {
           if (Service1 != null && Service1.Running) return true;
           if (Service2 != null && Service1.Running) return true;
           if (Service3 != null && Service1.Running) return true;
           return false;
       }

       private static void Listen()
       {//Here we are waiting for new conections to come in and can deal with a moximium of 3 clients at the same time
           Socket Soc = null;
           printDebug("Start listen", true);
           try
           {
               listener = new TcpListener(Settings.Port);//When microsoft is not changing code they are crashing old programs 
               listener.Start();
               while (THListen != null)
               {
                   Soc = listener.AcceptSocket();
                   printDebug("Connect " + Soc.RemoteEndPoint.ToString(), true);
                   string Msg = Settings.ScreenServerX + " " + Settings.ScreenServerY + " " + Screen.PrimaryScreen.Bounds.Width + " " + Screen.PrimaryScreen.Bounds.Height + " " + FirstConnect.ToString();
                   FirstConnect = false;
                   byte[] BMsg = ASCIIEncoding.ASCII.GetBytes(Msg);
                   Soc.Send(BMsg, BMsg.Length, SocketFlags.None);
                   if (Service1 == null || !Service1.Running) { Service1 = new Server(); Service1.Start(Settings.Port, Soc, Settings.ScreenServerX, Settings.ScreenServerY); }
                   else if (Service2 == null || !Service2.Running) { Service2 = new Server(); Service2.Start(Settings.Port, Soc, Settings.ScreenServerX, Settings.ScreenServerY); }
                   else if (Service3 == null || !Service3.Running) { Service3 = new Server(); Service3.Start(Settings.Port, Soc, Settings.ScreenServerX, Settings.ScreenServerY); }
               }
               listener.Stop();
           }
           catch (Exception Ex)
           {
               printDebug("Error listen " + Ex.Message, true);
           }
       }

       private static void printDebug(string Msg, bool Force)
       {
           if (Closing) return;
           if ((Settings.Debug || Force) && Settings.FormService != null)
           {
               Settings.FormService.Invoke((MethodInvoker)delegate()
               {//This code runs on the forms thread
                   Settings.FormService.printDebug(Msg, Force);
               });
           }
       }
       #endregion  //############## Start of our instance mebers used to process upto 3 conection at the same time ######################
       private int ScreenClientX = 1920;           //Sizes of the client and our screen
       private int ScreenClientY = 1080;
       private int ScreenServerX = 1920;
       private int ScreenServerY = 1080;
       private bool Encrypted = false;             //Flag for encryption on the screen, keyboard text always uses simple XOR encryption
       private bool Sleep = false;                 
       private int Padding = 3;
       private bool IsMetro = false;               //Flag for windows metro mode so we can nudge pen type devices to scroll the edge of the sceen 
       public int imageDelay = 2000;
       private Socket ServerSocket;                //Socket used for connection
       private int Port;                           //Port to listen on
       private Stream CStream;
       private Thread THEvents = null;             //Used to listen for commands coming in
       private Thread THServer = null;             //Used to push the destop image to the client
       public bool Running = false;
       private bool LoggedIn = false;
       private int SleepCount = 0;
       private bool Scale = false;                 //Set if we are scaling the size of the desktop image down to save bandwidth
       private bool Lock = false;                  //Set if the client has told us to shut up for a while
       private bool HadEvent = false;              //Set true if event sent from client
       private PixelFormat ImageResoloution = PixelFormat.Format16bppRgb555;// PixelFormat.Format32bppArgb;
       private DateTime LastEventTime = DateTime.Now;       //Stop talking to the service and it will close itself to save the CPU
       private string LastClipboardText = "";
       private Brush BrushWait = Brushes.Yellow;


       public void Start(int port,Socket serverSocket,int ScreenX,int ScreenY)
       {//Start new threads to send the desktop image and another to wait for keyboard/mouse commands to come in
           this.Scale = false;
           this.imageDelay = 2000;
           this.Sleep = false;
           this.ScreenServerX = ScreenX;
           this.ScreenServerY = ScreenY;
           this.ScreenClientX = Screen.PrimaryScreen.Bounds.Width;  //Windows seems to get this wrong sometimes but if it was made too easy for us
           this.ScreenClientY = Screen.PrimaryScreen.Bounds.Height; //then microsoft would not be selling us upgrades just so we can use windows remote desktop
           this.ImageResoloution = PixelFormat.Format16bppRgb555;   //We use 16 or 32 bit but not 64, it crashes and was too big anyway
           this.Running = true;
           this.LoggedIn = false; //Not logged in so we are not playing ball
           this.SleepCount = 0;
           this.ServerSocket = serverSocket;
           Settings.Port = port;
           this.Port = port;
           this.CStream = new NetworkStream(ServerSocket);
           LastClipboardText = ClipboardAsync.GetText();
           THEvents = new Thread(new ThreadStart(WaitForCommands));
           THEvents.Start();
           Thread.Sleep(300);  //Give the command thread a chance to start
           THServer = new Thread(new ThreadStart(PushTheDesktopToClients));
           THServer.Start();
       }

       public void Stop()
       {
           //Wallpaper.RestoreWallpaper();
           this.Running = false;
           Thread.Sleep(200);//Try to let things close down for the threads
           if (ServerSocket.IsBound)
               ServerSocket.Close();
           this.CStream = null;
           if (this.THEvents!=null) this.THEvents.Abort();//Don't know they say don't use Abort but what else can you do!
           if (this.THServer !=null) this.THServer.Abort();
       }

       public void SleepDelay()
       {//Sleep a bit so the network and the local CPU does not get over worked
           for (int f=0;f<=this.imageDelay/100;f++)
           {
                Thread.Sleep(100);
                if (HadEvent)//The client sent a mouse move or something so wake up and send the desktop back a bit early
                {
                    HadEvent = false;
                    return;
                }   
           }
       }

       private Bitmap ResizeImage(Bitmap B, int Width, int Height)
       {
           Bitmap BNew = new Bitmap(Width, Height);
           Graphics G = Graphics.FromImage(BNew);
           float sx = (float)Width / (float)B.Width;
           float sy = (float)Height / (float)B.Height;

           G.ScaleTransform(sx, sy);
           G.DrawImage(B, 0, 0);
           return BNew;
       }

       private void SendScreenInfo(string Data)
       {//The screen sizes need sending to the client
           Data = "#INFO#" + Data;
           if (this.Encrypted)
           {//Easy if a memory stream was always used and serialized but it comes at the price of performance, it all takes time
               BinaryFormatter bFormat = new BinaryFormatter();
               byte[] BClip = UTF8Encoding.UTF8.GetBytes(Helper.XorString(Data , 34, true));
               MemoryStream MS = new MemoryStream(BClip);
               bFormat.Serialize(CStream, MS);
               printDebug("INFO << Length=" + (Data.Length-6), true);
           }
           else
           {//When sent like this the client will catch an error and inform this service to lock, fixes the error and then tells this service to unlock
               Lock = true;//Client will send unlock message
               byte[] B = UTF8Encoding.UTF8.GetBytes("                                  " + Helper.XorString(Data.Replace(Environment.NewLine, "#NL#").Replace("\n", "#NL#").Replace("'", "#SQ#"), 34, true));
               CStream.Flush();
               CStream.Write(B, 0, B.Length);
               CStream.Flush();
               printDebug("INFO << Length=" + (Data.Length-6), true);
           }
       }

       public void SendClipboard()
       {
           string Text = ClipboardAsync.GetText();
           if (Text != null && Text == LastClipboardText)
               return ;
           LastClipboardText = Text;
           if (this.Encrypted && LastClipboardText.Length >0)
           {//Easy if a memory stream was always used and serialized but it comes at the price of performance, it all takes time
               BinaryFormatter bFormat = new BinaryFormatter();
               byte[] BClip = UTF8Encoding.UTF8.GetBytes(Helper.XorString ("#CLIPBOARD#" + LastClipboardText,34,true));
               MemoryStream MS = new MemoryStream(BClip);
               bFormat.Serialize(CStream, MS);
               printDebug("CLIPBOARD << Length=" + Text.Length, true);
           }
           else if (LastClipboardText.Length > 0)
           {//When sent like this the client will catch an error and inform this service to lock, fixes the error and then tells this service to unlock
               Lock = true;//Client will send unlock message
               byte[] B = UTF8Encoding.UTF8.GetBytes("                                  " + Helper.XorString("#CLIPBOARD#" + LastClipboardText.Replace(Environment.NewLine, "#NL#").Replace("\n", "#NL#").Replace("'","#SQ#"),34,true));
               CStream.Flush();
               CStream.Write(B, 0, B.Length);
               CStream.Flush();
               printDebug("CLIPBOARD << Length=" + Text.Length,true) ;
           }
       }


       public static MemoryStream Encrypt(Bitmap Img)//Tried using Rijndael but it thrashed the CPU on the server and the client flickered, look at using SSL if you need it
       {//We just remove the start of the PNG and put some junk in
           string Seed = DateTime.Now.Millisecond.ToString() + DateTime.Now.Hour + DateTime.Now.Second +"aqf";//"All queer fuckers" if you wanted to know
           byte []Dummey=UTF8Encoding.UTF8.GetBytes(Seed.Substring(0,6));
           MemoryStream MSin = new MemoryStream();
           Img.Save(MSin, ImageFormat.Png);
           MSin.Position = 0;
           MSin.Position = 0;
           MSin.Write(Dummey, 0, 6);
           return MSin; 
       }

       private void KeepMachineAwake()
       {//Fake key-move to keep the machine awake
           LastEventTime = DateTime.Now;
           SendKeys.SendWait("{DOWN}");
           Thread.Sleep(50);
           SendKeys.SendWait("{UP}");
       }

       private void PushTheDesktopToClients()
       {//Keep pushing the screen destop image to any clients that are awake
           string Stage = "Start";//Used so we know where things went wrong if something crashes 
           try
           {
               while (this.Running)
               {
                   TimeSpan TS = DateTime.Now - LastEventTime;
                   if (TS.TotalMinutes   > 10) KeepMachineAwake();
                   if (!this.Sleep && this.LoggedIn)//We sleep if the client has minimized the form
                   {
                       BinaryFormatter bFormat = new BinaryFormatter();
                       try
                       {
                           if (this.ScreenClientX != Screen.PrimaryScreen.Bounds.Width || this.ScreenClientY != Screen.PrimaryScreen.Bounds.Height) WriteScreenSize();
                           this.SleepCount = 0;
                           SendClipboard();
                           Bitmap screeny = new Bitmap(this.ScreenServerX + this.Padding, this.ScreenServerY + this.Padding, this.ImageResoloution);
                           Graphics theShot = Graphics.FromImage(screeny);
                           Stage = "Got Screen";
                           System.Drawing.Size Sz = new System.Drawing.Size(this.ScreenServerX + this.Padding, this.ScreenServerY + this.Padding);
                           theShot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, this.Padding, this.Padding, Sz, CopyPixelOperation.SourceCopy);
                           this.IsMetro = Helper.IsMetro(screeny);
                           if (this.Scale)//In effect we reduce the size of the image that we are sending back to the client
                               screeny = ResizeImage(screeny, this.ScreenServerX / 2, this.ScreenServerY / 2);
                           Stage = "Copy Screen";
                           int X = 0; int Y = 0;
                           Color CursorColor = WinCursor.CaptureCursor(ref X, ref Y, theShot, this.ScreenServerX, this.ScreenServerY);
                           screeny.SetPixel(0, 0, CursorColor);//Embed the curosr type in the image sent back to the cleint but need to be coded as a colour
                           if (this.IsMetro)
                               screeny.SetPixel(0, 1, Color.Black);//Embed "Black" in the image as flag for windows metro mode
                           else
                               screeny.SetPixel(0, 1, Color.Red);
                           if (!Lock)//Lock is sent by the client to stop us sending more data whilst it sorts itself out.
                           {
                               if (this.Encrypted)
                               {//Send as a encrypted memory stream
                                   MemoryStream MS = Encrypt(screeny);
                                   bFormat.Serialize(CStream, MS);
                               }
                               else//Send as a bit-map
                                   bFormat.Serialize(CStream, screeny);//This is a bit faster i thinks, hence the option

                           }
                           Stage = "Serialize Screen";
                           SleepDelay();//Pause for a bit but wake up early if the client sends a mouse click or something so we can send the desctop back early
                           theShot.Dispose();
                           screeny.Dispose();
                       }
                       catch (Exception Ex1)
                       {
                           if (!this.Running) return;
                           if (Ex1.Message.StartsWith("Unable to write data to the transport connection") || !this.ServerSocket.Connected)
                           {//Client may have closed without letting us know first 
                               this.THServer = null;
                               this.Stop();
                               return;
                           }
                           else if (Ex1.Message.StartsWith("The handle is invalid"))
                           {//Cannot get the screen when it is locked so flash a image
                               Bitmap screeny=(Bitmap)Server.ImgWinbackground ;
                               Graphics G = Graphics.FromImage(screeny);
                               if (BrushWait == Brushes.Yellow) BrushWait = Brushes.Orange; else BrushWait = Brushes.Yellow;
                               G.FillEllipse(BrushWait, 170, 100, 60, 60);
                               screeny.SetPixel(0, 0, Color.Fuchsia);
                               if (this.Encrypted)
                               {//Send as a encrypted memory stream
                                   MemoryStream MS = Encrypt(screeny);
                                   bFormat.Serialize(CStream, MS);
                               }
                               else//Send as a bit-map
                                   bFormat.Serialize(CStream, screeny);
                               Thread.Sleep(3000);
                           }
                           else
                                printDebug("Screen Error " + Stage + " " + Ex1.Message, true);
                           Thread.Sleep(50);
                           
                       }
                   }
                   else
                   {//We are in sleep mode or not logged in so do nothing
                       this.SleepCount++;
                       Thread.Sleep(500);
                       if (!this.LoggedIn && SleepCount>25)//We did not get a password sent
                       {
                           printDebug("NO LOGIN", true);
                           Stop();
                       }
                       else if (SleepCount > 7200)//One hour then we kill the session to save servers CPU
                       {
                           printDebug("Session timeout", true);
                           Stop();
                       }
                   }
               }
           }
           catch (Exception Ex)
           {//Big time crash, i'm drunk so i had better stop
               printDebug("Error Stop " + Ex.Message,true);
               Stop();
           }
       }

       private void TestPassword(string PassWord)
       {
           if (PassWord.Trim() == Settings.PassWord.ToLower() || Settings.PassWord.Length == 0)
               this.LoggedIn = true;
           else
           {
               printDebug("Bad password", true);
               this.Stop();
           }
       }
       
       private void ReadCommandValues(string temp)
       {//Could be a key-stroke or a mouse move/click or a cammon to do something else
           if (temp.StartsWith("CDELAY"))
           {
               printDebug(temp,false);
               this.imageDelay = int.Parse(temp.Substring(6, temp.Length - 6));
           }
          else if (temp.StartsWith("CMD "))
          {
              bool Force = false;
              string Cmd = temp.Substring(3).Trim();
              if (Cmd == "OK") return;
              if (Cmd.StartsWith("PASSWORD ")) { TestPassword(Cmd.Substring(9).ToLower()); Cmd = "PASSWORD *******"; Force = true; }
              if (Cmd.StartsWith("KEYSYNC ")) Helper.SyncKeys(Cmd); 
              if (Cmd == "SUP" && this.IsMetro) Helper.ScrollVertical(-50);
              if (Cmd == "SDOWN" && this.IsMetro) Helper.ScrollVertical(50);
              if (Cmd == "SLEFT" && this.IsMetro) Helper.ScrollHorizontal(-50);
              if (Cmd == "SRIGHT" && this.IsMetro) Helper.ScrollHorizontal(50);
              if (Cmd == "SHOWSTART") Helper.ShowStart();

              if (Cmd.StartsWith("PADDING ")) { this.Padding = int.Parse(Cmd.Replace("PADDING ", "").Trim()); }
              if (Cmd.StartsWith("DEBUG FALSE")) { printDebug(Cmd, false); ShowDebug(false); }
              if (Cmd.StartsWith("DEBUG TRUE")) { ShowDebug(true); Force = true; }
              if (Cmd.StartsWith("ENCRYPTED TRUE")) this.Encrypted = true;
              if (Cmd.StartsWith("ENCRYPTED FALSE")) this.Encrypted = false;
              if (Cmd.StartsWith("SLEEP TRUE")) this.Sleep  = true;
              if (Cmd.StartsWith("SLEEP FALSE")) this.Sleep = false;
              printDebug(Cmd, Force);
              if (Cmd.StartsWith("SCALE "))this.Scale = bool.Parse(Cmd.ToLower().Replace("scale ", ""));
              if (Cmd.StartsWith("RESOLUTION TRUE")) this.ImageResoloution = PixelFormat.Format32bppArgb;
              if (Cmd.StartsWith("RESOLUTION FALSE")) this.ImageResoloution = PixelFormat.Format16bppRgb555;
              //if (Cmd.StartsWith("WALLPAPER TRUE")) Wallpaper.SetWallpaper();
             // if (Cmd.StartsWith("WALLPAPER FALSE")) Wallpaper.RestoreWallpaper();
              if (Cmd.StartsWith("METRO")) Helper.ShowMetro();
              if (Cmd.StartsWith("CTRLALTDELETE")) Helper.ShowTaskmanager();
              if (Cmd.StartsWith("CLOSE")) this.Stop();
          }
           else if (temp.StartsWith("CLIPBOARD "))
           {
               LastClipboardText = temp.Substring(10).Replace("#NL#", Environment.NewLine).Replace("#SQ#","'");
               printDebug("CLIPBOARD >> Length=" + LastClipboardText.Length , false);
               ClipboardAsync.SetText(LastClipboardText);
           }
           else if (temp.StartsWith("SCREEN "))
           {
               printDebug(temp, false);
               this.ScreenServerX = int.Parse(temp.Split(' ')[1]);
               this.ScreenServerY = int.Parse(temp.Split(' ')[2]);
               Settings.ScreenServerX = this.ScreenServerX;
               Settings.ScreenServerY = this.ScreenServerY;
           }
           else if (temp.StartsWith("UNLOCK"))
           {//Can send more data
               printDebug(temp, false);
               this.Lock = false;
           }
           else if (temp.StartsWith("LOCK"))
           {//Don't send more data
               printDebug(temp, false);
               this.Lock = true;
           }
           else if (temp.StartsWith("BEEP"))
           {
               printDebug(temp, false);
               SystemSounds.Asterisk.Play();
           }
           else if (temp.StartsWith("SHUTDOWN"))
           {
               printDebug(temp, false);
               ListenStop(true);
           }
           else if (temp.StartsWith("{CAPSLOCK}"))
           {
               Helper.CapsLock();
               return;
           }
           else if (temp.StartsWith("{NUMLOCK}"))
           {
               Helper.NumLock();
               return;
           }
           else if (temp.StartsWith("LCLICK"))
           {
               //mouse_event(MOUSE_LEFTDOWN | MOUSE_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
               //printDebug(temp, false);
           }
           else if (temp.StartsWith("RCLICK"))
           {
               //mouse_event(MOUSE_LEFTDOWN | MOUSE_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
               //printDebug(temp, false);
           }
           else if (temp.StartsWith("LDOWN"))
           {
               Helper.mouse_event(Helper.MOUSEEVENTF_LEFTDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
           }
           else if (temp.StartsWith("LUP"))
           {
               Helper.mouse_event(Helper.MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
           }
           else if (temp.StartsWith("RCLICK"))
           {
               Helper.mouse_event(Helper.MOUSEEVENTF_RIGHTDOWN | Helper.MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
           }
           else if (temp.StartsWith("RDOWN"))
           {
               Helper.mouse_event(Helper.MOUSEEVENTF_RIGHTDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
           }
           else if (temp.StartsWith("RUP"))
           {
               Helper.mouse_event(Helper.MOUSEEVENTF_RIGHTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
           }
           else if (temp.StartsWith("M"))
           {
               int xPos = 0, yPos = 0;
               try
               {
                   xPos = int.Parse(temp.Substring(1, temp.IndexOf(' ')));
                   yPos = int.Parse(temp.Substring(temp.IndexOf(' '), temp.Length - temp.IndexOf(' ')));
                   Cursor.Position = new Point(xPos, yPos);
               }
               catch (Exception Ex)
               {
                   printDebug("Error Mouse " + Ex.Message, true);
               }
           }
           else
           {
               if (temp == "^c") printDebug("{COPY}", true);
               else if (temp == "^v") printDebug("{PASTE}", true);
               else if (temp.Length >1) printDebug(temp, true);
               SendKeys.SendWait(temp);
           }
       }

       private void WriteScreenSize()
       {
           SendScreenInfo("SCREEN_" + Screen.PrimaryScreen.Bounds.Width + "_" + Screen.PrimaryScreen.Bounds.Height);
           this.ScreenClientX = Screen.PrimaryScreen.Bounds.Width;
           this.ScreenClientY = Screen.PrimaryScreen.Bounds.Height;
           this.ScreenServerX = Screen.PrimaryScreen.Bounds.Width;
           this.ScreenServerY = Screen.PrimaryScreen.Bounds.Height;
       }

       private void WaitForCommands()
       {//Runs on its own thread and we just wait for the client to send us commands for the keyboard or mouse or maybe telling us to shut up, well shut down
           StreamReader reader = new StreamReader(CStream);
           while (this.Running && !Closing)
           {
               if (this.Sleep) Thread.Sleep(1000);
                try
                {
                    if (this.ScreenClientX != Screen.PrimaryScreen.Bounds.Width || this.ScreenClientY != Screen.PrimaryScreen.Bounds.Height) WriteScreenSize();
                    int Shift = 45;
                    string temp = reader.ReadLine();
                    if (temp.Length == 1)
                        Shift = 77;//Change our encryption shift if it's just a keystroke to make it harder to listen in on the conversation
                    temp = Helper.XorString(temp, Shift, false);//Could use more heavey weight encryption here since the size is small
                    if (temp == "C33C") temp = " ";//What the hell was i doing here, Who wrote this code anyway :)
                    LastEventTime = DateTime.Now;  //Stop talking to us for a long time and we will shut down the connection !
                    if (temp != null)
                    {
                        ReadCommandValues(temp);//Processs the command we just got
                        HadEvent = true;
                    }
                }
                catch (Exception Ex)
                {
                    if (!this.ServerSocket.Connected && !Closing && this.Running)
                    {
                        printDebug("Error waitForKeys " + Ex.Message, true);
                        this.THEvents = null;
                        this.Stop();
                        return;
                    }
                }                
           }
       }
    }
}
