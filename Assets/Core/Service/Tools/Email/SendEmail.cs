
 using UnityEngine;
 using System.Collections;
 using System;
 using System.Net;
 using System.Net.Mail;
 using System.Net.Security;
 using System.Security.Cryptography.X509Certificates;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class SendEmail : MonoBehaviour
{

    static SendEmail instance;
    public static SendEmail LoadTemplate(string name)
    {
        if (instance == null)
        {
            var template = (GameObject)Resources.Load(name);
            if (template != null)
            {
                var ins = Instantiate(template);
                instance = ins.GetComponent<SendEmail>();
            }
        }
        return instance;
    }
    public void Close() 
    {
        Destroy(instance.gameObject);
    }









    [Header("====== Setting ======")]
    public string SenderEmail;
    public string SenderPassword;
    public string ToEmail;
    public string DisplayName;
    public string Subject;
    public string Host = "smtp.gmail.com";
    public int Port = 587;
    public TextAsset temp;

    public void Send()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(SenderEmail, DisplayName );
        mail.To.Add(ToEmail);
        mail.Subject = Subject;
        mail.Body = html();
        //Debug.Log("html = " + html());
        mail.IsBodyHtml = true;
        SmtpClient smtpServer = new SmtpClient(Host);
        smtpServer.Port = Port;
        smtpServer.Credentials = new System.Net.NetworkCredential( SenderEmail , SenderPassword ) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");

    }

  

    [Header("====== Body Content ======")]
    public string image_cover_url;
    public string header;
    public string[] messages;
    public bool isBtn;
    public string btn_url;
    public string btn_name;
    public string foot;
    public string footmessage;
    public void ReplaceMessages(string key , string message)
    {
        for (int i = 0; i < messages.Length; i++) 
        {
            messages[i] = messages[i].Replace(key, message);
        }
    }
    public string html() {
        string original = temp.text;





        original = original.Replace("#@image_cover_url#", image_cover_url);
        original = original.Replace("#@header#", header);


        string message = "";
        foreach (var msg in messages) {
            message += msg + " <br>";
        }
        original = original.Replace("#@message#", message);

        original = original.Replace("#@foot#", foot);
        original = original.Replace("#@footmessage#", footmessage);


        if (!isBtn)
        {
            original = Service.String.strCropRemove(original, "#<btn#", "#btn>#");
        }
        else 
        {
            original = original.Replace("#@btn_url#", btn_url);
            original = original.Replace("#@btn_name#", btn_name);
        }
        original = original.Replace("#<btn#", "");
        original = original.Replace("#btn>#", "");


        return original;
    }


}

















#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(SendEmail))]
[System.Serializable]
public class SendEmailUI : Editor
{

    string filename = "emailpreview.html";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mail = (SendEmail)target;



        if (GUILayout.Button("Send")) 
        {
            mail.Send();
        }
        if (GUILayout.Button("Preview"))
        {
            System.IO.File.WriteAllText(filename, mail.html());
            Application.OpenURL(filename);
        }

       if (System.IO.File.Exists(filename)) {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Remove Preview"))
            {
                System.IO.File.Delete(filename);
            }
            GUI.backgroundColor = Color.white;
        }


    }
}
#endif