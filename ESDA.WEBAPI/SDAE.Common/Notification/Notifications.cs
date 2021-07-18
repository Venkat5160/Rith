using SDAE.Data.Model;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SDAE.Common.Notification
{
    public class Notifications : INotification
    {
        public async Task FireBaseIOSNotification(PushNotificationDto model)
        {
            try
            {
                var data = new
                {
                    to = model.DeviceToken,
                    notification = new
                    {
                        body = model.Message,
                        title = model.Title,
                        transactionId = model.TransactionId,
                        token = model.Token
                    },
                    priority = "high"
                };

                if (data != null)
                {
                    WebRequest request = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    request.Method = "post";
                    request.ContentType = "application/json";

                    var json = JsonSerializer.Serialize(data);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    request.Headers.Add(string.Format("Authorization: key={0}", model.IOSAppId));
                    request.ContentLength = byteArray.Length;

                    using Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using WebResponse response = request.GetResponse();
                    using Stream dataStreamResponse = response.GetResponseStream();
                    using StreamReader reader = new StreamReader(dataStreamResponse);
                    String sResponseFromServer = reader.ReadToEnd();
                    string result = sResponseFromServer;

                    var res = Newtonsoft.Json.JsonConvert.DeserializeObject<PushNotificationResponse>(result);
                    if (res != null)
                    {
                        if (res.success == "1")
                        {
                            //"Notification sent successfully";
                        }
                        else
                        {
                            //"Error occured while sending Notification";
                        }
                    }
                    else
                    {
                        //"Error occured while sending Notification";
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                //Log.Error(cons"Error tracked at BatchEngine.Run(), exception details: ");
                //Log.ErrorFormat("Message : {0} ", ex.Message);
                //Log.ErrorFormat("StackTrace : {0} ", ex.StackTrace);
                //Log.ErrorFormat("InnerException : {0} ", ex.InnerException);
            }
        }

        public async Task IOSNotification(PushNotificationDto model)
        {
            DroidNotification(model);

            int port = model.IOSPort;
            string hostname = model.IOSHost;
            string certificatePassword = model.IOSPassword;
            string certificatePath = model.IOSCertificatePath;
            TcpClient client = new TcpClient(hostname, port);
            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), certificatePassword);
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);
            SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, false);
            string deviceToken = model.DeviceToken;
            string transactionId = model.TransactionId;
            int Counter = 1;//Badge Count;  
            string message = model.Message;
            string token = model.Token;
            var payload = "{\"aps\":{\"alert\":\"" + message + "\",\"badge\":" + Counter + ",\"sound\":\"default\"},\"token\":\"" + token + "\",\"transactionId\":\"" + transactionId + "\"}";
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)32);
            writer.Write(HexStringToByteArray(deviceToken.ToUpper()));
            writer.Write((byte)0);
            writer.Write((byte)payload.Length);
            byte[] b1 = Encoding.UTF8.GetBytes(payload);
            writer.Write(b1);
            writer.Flush();
            byte[] array = memoryStream.ToArray();
            sslStream.Write(array);
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            return false;
        }

        public void DroidNotification(PushNotificationDto model)
        {
            model.DeviceToken = "cOo7v-8mRvmveMWQ9XTY1K:APA91bEVJE7rQ8XMhcIi9yOTEpKB9j2wlYMBN3_sri6N2vq5gSsNw7Q3AVmrIbJZt8E23kjTesD4YbF-xWyHJxdRiSm1DqqMSsDK6W_4vUHm7i3QTJ28aInTLxRpgI4PWBgA5t4TZwd0";
            try
            {
                var data = new
                {
                    to = model.DeviceToken,
                    notification = new
                    {
                        body = model.Message,
                        title = model.Title,
                        transactionId = model.TransactionId,
                        token = model.Token
                    },
                    priority = "high"
                };


                if (data != null)
                {
                    WebRequest wRequest;
                    var value = JsonSerializer.Serialize(data);
                    //value = "{'title'                   :'Test Title',"
                    //            + "'body'              :'Test body',"
                    //            + "'action_tag'        :'test',"
                    //            + "'message'           :'test msg',"
                    //            + "'notification_type' :'1',"
                    //            + "'notification_data' :'',"
                    //            + "'sound'             :'default'} ";

                    wRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");//latest version

                    wRequest.Method = "post";
                    wRequest.ContentType = " application/json;charset=UTF-8";
                    wRequest.Headers.Add(string.Format("Authorization: key={0}", model.DroidAppId));
                    Byte[] bytes = Encoding.UTF8.GetBytes(value);
                    wRequest.ContentLength = bytes.Length;

                    Stream stream = wRequest.GetRequestStream();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                    WebResponse wResponse = wRequest.GetResponse();
                    stream = wResponse.GetResponseStream();

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string response = reader.ReadToEnd();
                        HttpWebResponse httpResponse = (HttpWebResponse)wResponse;
                        string status = httpResponse.StatusCode.ToString();
                        reader.Close();
                        stream.Close();
                        wResponse.Close();

                        var res = Newtonsoft.Json.JsonConvert.DeserializeObject<PushNotificationResponse>(response);
                        if (res != null)
                        {
                            if (res.success == "1")
                            {
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                        Console.WriteLine(response);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
