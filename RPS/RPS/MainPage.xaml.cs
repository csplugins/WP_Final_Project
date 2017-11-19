using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RPS
{
    /// Written by Timothy Hopkins
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// Serialization code found from https://msdn.microsoft.com/en-us/library/system.runtime.serialization.datacontractattribute(v=vs.110).aspx
    ///
    public sealed partial class MainPage : Page
    {
        RPSAI ai;
        int result = -1;
        private const string path = "rspai.xml";

        public MainPage()
        {
            this.InitializeComponent();
            // This is the try catch block for serializing the code
            // try and load the serialized class

            try
            {
                ai = new RPSAI();
                Task task = RPS.My_Serialize.CreateObject<RPSAI>(ai, path);
            }
            // If there is an error we need to detect which kind.
            // If the serialized file doesn't exist then we need to
            // create one.
            catch(FileNotFoundException)
            {
                ai = new RPSAI();
                Task task = RPS.My_Serialize.SaveObject<RPSAI>(ai, path);
            }
            // If there is another type of error then we need to catch that.
            catch(SerializationException ex)
            {
                Result.Text = ex.Message;
            }
            finally
            {
                Task<RPSAI> task = RPS.My_Serialize.ReadObject<RPSAI>(path);
                ai = task.Result;
            }
        }

        private void rock_Click(object sender, RoutedEventArgs e)
        {

            result = ai.choose(0);
            display_result(result);
        }

        private void paper_Click(object sender, RoutedEventArgs e)
        {
            result = ai.choose(1);
            display_result(result);
        }

        private void scissors_Click(object sender, RoutedEventArgs e)
        {
            result = ai.choose(2);
            display_result(result);
        }

        private void display_result(int result)
        {
            switch(result)
            {
                case 0:
                    Result.Text = "You Win!";
                    break;
                case 1:
                    Result.Text = "You Lose!";
                    break;
                case 2:
                    Result.Text = "It is a Tie!";
                    break;
                case -1:
                    Result.Text = "Oh No! Error!";
                    break;
                default:
                    Result.Text = "This shouldn't happen.";
                    break;
            }
        }
    }
}
