//Too lazy to install github client, just create a VS C# Console project named Pttcheck (or replace namespace on this file on line 12) and replace program.cs with this file.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Pttcheck
{
    class Program
    {
        static WebClient wc = new WebClient();
        static void Main(string[] args)
        {
            InitiateSSLTrust(); //SSL sertifikaları olmadığından geçmek zorundayız :(
            if (File.Exists("config.ini"))
            {
                var config = File.ReadAllLines("config.ini");
                var printoutput = false;
                wc.Headers.Add("user-agent", "Dalvik/1.4.0 (Linux; U; Android 2.3.3; sdk Build/GRI34)"); //Useragent kontrolü yazpıyor
                var barkod = config[0]; //Kodu buraya yazın, örnek: RA123456789SG
                var veri = "test";
                while (true)
                {
                    var gelenveri = wc.DownloadString("http://212.175.152.18/cepptt/android/posta/yurtDisiKargoSorgula?barkod=" + barkod);
                    if (gelenveri != veri && veri != "")
                    {
                        Console.WriteLine("Fark bulundu!");
                        MailAt("Gönderide fark bulundu. JSON: " + gelenveri);
                    }
                    else
                    {
                        Console.WriteLine("Fark bulunamadı!");
                    }
                    if (printoutput)
                    {
                        Console.WriteLine(gelenveri);
                    }
                    veri = gelenveri;
                    System.Threading.Thread.Sleep(60 * 60 * 1000); //1 saat
                }
            }
            else
            {
                File.WriteAllText("config.ini", string.Format("barkod-buraya{0}gonderen-email@buraya.com{0}alici-email@buraya.com{0}gonderen-mail-sifre-buraya", Environment.NewLine));
                Console.WriteLine("Lütfen config.ini dosyasını düzenleyin.");
            }
        }

        public static void MailAt(string icerik)
        {
            try
            {
                var config = File.ReadAllLines("config.ini");
                MailAddress from = new MailAddress(config[1]);
                MailAddress to = new MailAddress(config[2]);
                MailMessage mail = new MailMessage(from, to);
                mail.Subject = "PTT Kontrol";
                mail.Body = icerik;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(config[1], config[3]);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                Console.WriteLine("Mail gönderildi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mailde hata :( Sebep: " + ex.Message);
                //Tembellik
            }
        }

        public static void InitiateSSLTrust()
        {
            try
            {
                //Change SSL checks so that all checks pass
                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );
            }
            catch (Exception ex)
            {

            }
        }

    }
}
