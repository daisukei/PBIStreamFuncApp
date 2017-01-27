using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public static void Run(string myQueueItem, TraceWriter log)
{
    log.Info($"Power BI streaming API from Queue trigger function processed: {myQueueItem}");

    string realTimePushURL = "[Power BI Stream API]";

    while (true)
    {
        String currentTime = DateTime.UtcNow.ToString();
        Random r = new Random();
        int scoreLabel = r.Next(0, 100);

        log.Info($"scoreLabel = {scoreLabel}");

        WebRequest request = WebRequest.Create(realTimePushURL);
        request.Method = "POST";
        string postData = String.Format("[{{ \"currentTime\": \"{0}\", \"scoreLabel\":{1} }}]", currentTime, scoreLabel);

        // sending request to Power BI streaming API
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        request.ContentLength = byteArray.Length;
        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        WebResponse response = request.GetResponse();

        // Get the stream containing content returned by the server.
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();

        // Clean up the streams
        reader.Close();
        dataStream.Close();
        response.Close();

        // Wait 1 second 
        System.Threading.Thread.Sleep(1000);
    }
}