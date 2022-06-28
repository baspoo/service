using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

public class SendEmail : MonoBehaviour {
	[Header("** Must Set Api Compatibility Level to .Net 2.0")]
	[Header("Email/password is in hardcode.")]
	[Header("For gmail have to enable low secure mode in account setting.")]

	//** Setup
	private string sSenderEmail = "baspoogamedev@gmail.com";
	private string sSenderEmailPassword = "baspoo053955250";
	private string sDisplayNameFrom = "Baspoo";
	private string sSubject = "Test Mail";















	public bool bReadyToSend = true;
	public string txtError = string.Empty;
	public delegate void Callback ( bool successfully );
	Callback m_callback;
	public void sendEmail(string Eamil , byte[] b , Callback callback){
		if (bReadyToSend) {
			bReadyToSend = false;
			m_callback = callback;
			StartCoroutine(DoSendEmail (Eamil,b));
		}
	}
	IEnumerator DoSendEmail(string Eamil  , byte[] b){
		yield return new WaitForSeconds(0.5f);
		Debug.Log("DoSendEmail to "+Eamil);
		MailMessage mail = new MailMessage();
		bool bError = false;
		#region Image_BodyHtml
			string PicName = "Pic"+Random.Range(0,99999).ToString();
			System.IO.Stream Stream = new System.IO.MemoryStream (b); 
			string htmlBody ="";
			htmlBody+="<html><head><title></title></head><body>";
//			htmlBody+="<p style=\"text-align: center;\"><span style=\"color:#00ccff;\"><span style=\"font-size:26px;\"><strong>THANK YOU FOR VISITING.</strong></span></span></p>";
//			htmlBody+="<p style=\"text-align: center;\"><span style=\"color:#00ccff;\"><span style=\"font-size:26px;\"><strong>PLEASE FIND YOUR PHOTO IN THIS EMAIL.</strong></span></span></p>";
//			htmlBody+="<p style=\"text-align: center;\"><strong><span style=\"font-size:8px;\">&nbsp;</span></strong></p>";
//			htmlBody+="<p style=\"text-align: center;\"><strong><span style=\"font-size:16px;\">WE HOPE YOU HAD A FANTASTIC TIME AT OUR EVENT.</span></strong></p>";
//			htmlBody+="<p style=\"text-align: center;\"><strong><span style=\"font-size:16px;\">AND WE LOOK FORWARD TO SEE YOU AGAIN NEXT YEAR!</span></strong></p>";
//			htmlBody+="<p style=\"text-align: center;\"><strong><span style=\"font-size:2px;\">&nbsp;</span></strong></p>";
			htmlBody+="<p style=\"text-align: center;\"><img src=\"cid:"+PicName+"\"></p>";
			htmlBody+="</body></html>";
			AlternateView avHtml = AlternateView.CreateAlternateViewFromString
				(htmlBody, null, MediaTypeNames.Text.Html);
			
			LinkedResource pic1 = new LinkedResource(Stream,MediaTypeNames.Image.Jpeg);
			pic1.ContentId = PicName;
			pic1.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
			avHtml.LinkedResources.Add(pic1);
		#endregion



		mail.From = new MailAddress(sSenderEmail,sDisplayNameFrom);
		mail.AlternateViews.Add (avHtml);
		mail.To.Add(Eamil);
		mail.Subject = sSubject;
		mail.IsBodyHtml = true;
		mail.Body = htmlBody;
		SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential(sSenderEmail, sSenderEmailPassword) as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { 
			Debug.Log(">> Email, ServerCertificateValidationCallback (???)");
			return true;
			};

		try {
			smtpServer.SendAsync(mail,gameObject);
			Debug.Log(">> Better check your email send out in email account. The Email may error without show any error here.");
			StartCoroutine(AfterSend());
		}  
		catch ( SmtpException ex) {
			string err = ex.ToString();
			Debug.Log(">> Email, Error : "+err );
			if( err.Contains("failed recipients") ){
				txtError = "Sorry, Please check email address";
			}else{
				txtError = "Sorry, Please check your internet connection.";
			}
			StartCoroutine(AfterSend());
		}
		yield break; 
	}

	IEnumerator AfterSend(){
		yield return new WaitForSeconds(1f);
		bReadyToSend = true;
		if (txtError != string.Empty) {
			Debug.Log (txtError);
			m_callback (false);
		}
		else {
			Debug.Log ("Email, success");
			m_callback (true);
		}
	}
}
