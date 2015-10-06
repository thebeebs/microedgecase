using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCasesApp
{
    class EdgeCaseModel
    {

        public async static Task<RootObject> GetResults(string url)
        {
            var http = new HttpClient();
            var apiUrl = String.Format("http://edge.azurewebsites.net/api/v2/scan?url=http://{0}", url);
            var response = await http.GetAsync(apiUrl);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }

    }

    [DataContract]
    public class Url
    {
        [DataMember]
        public string uri { get; set; }
    }

    [DataContract]
    public class Datum
    {
        [DataMember]
        public bool passed { get; set; }
        [DataMember]
        public string pattern { get; set; }
        [DataMember]
        public int lineNumber { get; set; }
        [DataMember]
        public string url { get; set; }
    }

    [DataContract]
    public class Javascript
    {
        [DataMember]
        public bool passed { get; set; }
        [DataMember]
        public List<Datum> data { get; set; }
    }

    [DataContract]
    public class Comments
    {
        [DataMember]
        public bool passed { get; set; }
    }

    [DataContract]
    public class Data
    {
        [DataMember]
        public Javascript javascript { get; set; }
        [DataMember]
        public Comments comments { get; set; }
    }

    [DataContract]
    public class BrowserDetection
    {
        [DataMember]
        public string testName { get; set; }
        [DataMember]
        public bool passed { get; set; }
        [DataMember]
        public Data data { get; set; }
    }

    [DataContract]
    public class CssPrefixes
    {
        [DataMember]
        public string testName { get; set; }
        [DataMember]
        public bool passed { get; set; }
        [DataMember]
        public List<object> data { get; set; }
    }

    [DataContract]
    public class Data2
    {
        [DataMember]
        public int lineNumber { get; set; }
        [DataMember]
        public List<string> mode { get; set; }
    }

    [DataContract]
    public class Edge
    {
        [DataMember]
        public string testName { get; set; }
        [DataMember]
        public bool passed { get; set; }
        [DataMember]
        public Data2 data { get; set; }
    }

    [DataContract]
    public class JSLibs
    {
        [DataMember]
        public string testName { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public bool passed { get; set; }
        [DataMember]
        public List<object> data { get; set; }
    }

    [DataContract]
    public class PluginFree
    {
        [DataMember]
        public string testName { get; set; }
        [DataMember]
        public bool passed { get; set; }
        [DataMember]
        public List<object> data { get; set; }
    }

    [DataContract]
    public class Datum2
    {
        [DataMember]
        public string element { get; set; }
        [DataMember]
        public double threshold { get; set; }
        [DataMember]
        public int edgeCount { get; set; }
        [DataMember]
        public int chromeCount { get; set; }
        [DataMember]
        public bool passed { get; set; }
    }

    [DataContract]
    public class Markup
    {
        [DataMember]
        public string testName { get; set; }
        [DataMember]
        public bool passed { get; set; }
        [DataMember]
        public List<Datum2> data { get; set; }
    }

    [DataContract]
    public class Results
    {
        [DataMember]
        public BrowserDetection browserDetection { get; set; }
        [DataMember]
        public CssPrefixes cssprefixes { get; set; }
        [DataMember]
        public Edge edge { get; set; }
        [DataMember]
        public JSLibs jslibs { get; set; }
        [DataMember]
        public PluginFree pluginfree { get; set; }
        [DataMember]
        public Markup markup { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public Url url { get; set; }
        [DataMember]
        public double processTime { get; set; }
        [DataMember]
        public string machine { get; set; }
        [DataMember]
        public Results results { get; set; }
    }
}
