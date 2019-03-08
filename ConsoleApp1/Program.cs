using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
           
            string a = "";
            string b = "";
            string c = "";
            string d = "";
            Console.WriteLine("enter house number");
            a=Console.ReadLine();
            Console.WriteLine("enter street name");
            b = Console.ReadLine();
            Console.WriteLine("enter enter post code");
            c = Console.ReadLine();
            Console.WriteLine("enter city");
            d = Console.ReadLine();

            // The url or service we wish to call
            string url = "https://dawa.aws.dk/autocomplete?caretpos=28&fuzzy=&q="+b+"+"+a+",+"+c+"+"+d+"&startfra=adresse&type=adresse";
            // The result from the call
            string text = "";
            // Create a request to the URL
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // Get the text from the URL
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                // Read all the data from the URL by a stream
                text = readStream.ReadToEnd();
                // Close the response and the stream
                response.Close();
                readStream.Close();
            }
            // Parse and make a list of postcodes
            var postcodeList = JArray.Parse(text).
            Select(p =>
            new
            {
                address = (string)p["tekst"],
              
            });
            // Make an string to hold the SQL 
            string str = "";
            // Make the SQL from the list of data we have
            foreach (var post in postcodeList)
            {
                str += "INSERT INTO Postnumbers (Postnr, City)" + System.Environment.NewLine +
                "VALUES ('" +post.address + "');" + System.Environment.NewLine;
            }

            //str.Dump();
            Console.WriteLine(str);
            Console.ReadKey();
        }
    }
}
