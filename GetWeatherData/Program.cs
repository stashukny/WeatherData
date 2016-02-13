﻿using System;
using System.Text;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace RESTServicesJSONParser
{
    class Program
    {
        static string PostalCode, TimeWindow, Folder;
        static string Key = "ef8b4180c04fa4bf8c4f";
        static string HeaderFormat = "{0},{1},{2},{3},{4},{5},{6},{7}";
        static string Headers = "postal_code,timestamp,tempMin,tempAvg,tempMax,cldCvrAvg,snowfall,precip";
        static void Main(string[] args)
        {
            try
            {
                foreach (string arg in args)
                {
                    ParseArgument(arg);
                }

                string locationsRequest = CreateRequest();
                List<Response> locationsResponse = MakeRequest(locationsRequest);
                ProcessResponse(locationsResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }

        }

        static void ParseArgument(string arguments)
        {
            string[] argument = arguments.Split('=');
            if (argument != null && argument.Length == 2)
            {
                switch (argument[0].ToLower())
                {
                    case "pc":
                        PostalCode = argument[1].ToString();                    
                        break;
                    case "tw":
                        TimeWindow = argument[1].ToString();                      
                        break;
                    case "fld":
                        Folder = argument[1].ToString();
                        break;   
                }
            }
        }

        public static string CreateRequest()
        {
            //string UrlRequest = "https://api.weathersource.com/v1/" + Key + "/history_by_postal_code.json?limit=25&period=day&postal_code_eq=" + PostalCode + "&country_eq=US&timestamp_between=" + TimeWindow + "&fields=postal_code,country,timestamp,tempMax,tempAvg,tempMin,precip,snowfall,windSpdMax,windSpdAvg,windSpdMin,cldCvrMax,cldCvrAvg,cldCvrMin,dewPtMax,dewPtAvg,dewPtMin,feelsLikeMax,feelsLikeAvg,feelsLikeMin,relHumMax,relHumAvg,relHumMin,sfcPresMax,sfcPresAvg,sfcPresMin,spcHumMax,spcHumAvg,spcHumMin,wetBulbMax,wetBulbAvg,wetBulbMin";
            string UrlRequest = "https://api.weathersource.com/v1/" + Key + "/history_by_postal_code.json?limit=25&period=day&postal_code_eq=" + PostalCode + "&country_eq=US&timestamp_between=" + TimeWindow + "&fields=postal_code,timestamp,tempMax,tempAvg,tempMin,precip,snowfall,cldCvrAvg";
            return (UrlRequest);
        }

        public static List<Response> MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Response>));
                    Stream s = response.GetResponseStream();

                    List<Response> objResponse = (List<Response>)jsonSerializer.ReadObject(s);
                    if (objResponse.Count == 0)
                        throw new Exception("empty record set");
                    return objResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static public void ProcessResponse(List<Response> locationsResponse)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Headers);

            for (int i = 0; i < locationsResponse.Count; i++)
            {
                sb.AppendFormat(HeaderFormat, "\"" + locationsResponse[i].postal_code.ToString() + "\"", "\"" + locationsResponse[i].timestamp.ToString().Replace("T00:00:00-05:00", "") + "\"",
                                                        locationsResponse[i].tempMin.ToString(), locationsResponse[i].tempAvg.ToString(), locationsResponse[i].tempMax.ToString(),
                                                        locationsResponse[i].cldCvrAvg.ToString(), locationsResponse[i].snowfall.ToString(), locationsResponse[i].precip.ToString());
                sb.AppendLine();
            }

            string FileName = Folder + "\\" + PostalCode + "_" + TimeWindow.Replace("T00:00:00+00:00", "").Replace(".", "_").Replace(",", "_") + "_" + DateTime.Now.ToShortDateString().Replace("/", "") + ".csv";
            System.IO.File.WriteAllText(FileName, sb.ToString());          
        }
    }
}


