using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Program
    {
        //set up the urls
        public static string xmlURL = "https://jonpaulsulli1009.github.io/CSE445_P4try2/Hotels.xml"; //Q1.2 
        public static string xmlErrorURL = "https://jonpaulsulli1009.github.io/CSE445_P4try2/HotelsErrors.xml"; //Q1.3
        public static string xsdURL = "https://jonpaulsulli1009.github.io/CSE445_P4try2/Hotels.xsd"; //Q1.1
        //added some vars
        private static bool errorFree;
        private static StringBuilder errorString = new StringBuilder();
        private static int errorCount = 0;
        private static List<Hotel> myHotels;

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //Setup xml schema set, xml reader setting and vars
            XmlSchemaSet xss = new XmlSchemaSet();
            XmlReaderSettings xrs = new XmlReaderSettings();
            errorFree = true;
            errorCount = 0;
            errorString.Clear();

            try{
                //add the xsd to our schema set
                xss.Add(null, xsdUrl);
            
                //fix the reader settings and give it the schema
                xrs.ValidationType = ValidationType.Schema;
                xrs.Schemas = xss;

                //add my callback
                xrs.ValidationEventHandler += new ValidationEventHandler(MyValidationCallBack);

                //make a reader and read the xml
                XmlReader xr = XmlReader.Create(xmlUrl,xrs);
                while(xr.Read());
            }
            
            Console.WriteLine("validation complete");
            
            if(errorFree){
                return "No Error";
            }else{
                return errorString.ToString();
            }
        }

        private static void MyValidationCallBack(object sender, ValidationEventArgs e){
            errorFree = false;
            errorCount++;
            errorString.Append("Error {errorCount}: {e.Message}\n");
        } 

        public static string Xml2Json(string xmlUrl)
        {
            //get stuff ready
            XmlReader xr = XmlReader.Create(xmlUrl);
            myHotels = new List<Hotel>();
            Hotel curHotel = null;
            Address curAddr = null;
            List<String> curPhones = null;
            //start reading
            while(xr.Read()){
                if(xr.NodeType == XmlNodeType.Element && xr.Name == "Hotel"){
                    curHotel = new Hotel();
                    curPhones = new List<string>();
                    curAddr = new Address();
                    curHotel.rate = xr.GetAttribute("Rating");
                    XmlReader hotelSub = xr.ReadSubtree();
                    while(hotelSub.Read()){
                        switch(hotelSub.Name){
                            case "Name":
                                curHotel.name = hotelSub.ReadElementContentAsString();
                                break;
                            case "Phone":
                                curHotel.phones.Add(hotelSub.ReadElementContentAsString());
                                break;
                            case "Address":
                                curAddr.nearair = hotelSub.GetAttribute("NearestAirport");
                                using(xmlReader addrSub = hotelSub.ReadSubtree()){
                                    while(addrSub.Read()){
                                        switch(addrSub.Name){
                                            case "Number":
                                                curAddr.snumber = addrSub.ReadElementContentAsString();
                                                break;
                                            case "Street":
                                                curAddr.snumber = addrSub.ReadElementContentAsString();
                                                break;
                                            case "City":
                                                curAddr.snumber = addrSub.ReadElementContentAsString();
                                                break;
                                            case "State":
                                                curAddr.snumber = addrSub.ReadElementContentAsString();
                                                break;
                                            case "Zip":
                                                curAddr.snumber = addrSub.ReadElementContentAsString();
                                                break;
                                        }
                                    }
                                }
                                break;
                        }
                    }     
                } else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "Hotel"){
                    curHotel.phones = curPhones;
                    curHotel.addr = curAddr;
                    myHotles.Add(curHotel);
                {
            }
            Hotels allHotels = new Hotels();
            allHotels.Hotel = myHotels;
            JsonSerializerOptions options = new JsonSerializerOptions{WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull};
            string jsonText = JsonSerializer.Serialize(allHotels, options);
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            return jsonText;

        }
        private class Hotels{
            public List<Hotel> Hotel;
        }
        private class Hotel{ 
            public string name;
            public List<string> phones;
            public Address addr;
            public String rate;
        }
        private class Address{
            public string snumber;
            public string street;
            public string city;
            public string state;
            public string zip;
            public string nearair;
        }
    }

}






