using System;
using System.Collections.Generic;
using System.Threading ;
using System.Windows.Forms;

namespace RemoteClient
{
    public class ClipboardAsync
    {

        private string _GetText;
        private string _SetText;
        private void _thSetText()
        {
            if (_SetText.Length == 0) return;
            Clipboard.Clear();
            Clipboard.SetText(_SetText);
        }
        private void _thGetText(object format)
        {
            try
            {
                if (format == null)
                {
                    _GetText = Clipboard.GetText();
                }
                else
                {
                    _GetText = Clipboard.GetText((TextDataFormat)format);

                }
            }
            catch (Exception ex)
            {
                //Throw ex 
                _GetText = string.Empty;
            }
        }
        public static string GetText()
        {
            ClipboardAsync instance = new ClipboardAsync();
            Thread staThread = new Thread(instance._thGetText);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return instance._GetText;
        }

        public static void SetText(string Text)
        {
            ClipboardAsync instance = new ClipboardAsync();
            instance._SetText = Text;
            Thread staThread = new Thread(instance._thSetText);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
        }
        public string GetText(TextDataFormat format)
        {
            ClipboardAsync instance = new ClipboardAsync();
            Thread staThread = new Thread(instance._thGetText);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start(format);
            staThread.Join();
            return instance._GetText;
        }
    }
}
