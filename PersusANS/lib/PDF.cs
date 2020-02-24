using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PersusANS.lib{

    class PDF{

        private void sign(X509Certificate2 cert, String imput, String output, int posicaoAssinatura)
        {
            string SourcePdfFileName = imput;
            string DestPdfFileName = output;
            string requerente = "";
            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };
            IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");
            PdfReader pdfReader = new PdfReader(SourcePdfFileName);
            FileStream signedPdf = new FileStream(DestPdfFileName, FileMode.Create);  //the output pdf file
            PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');
            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

            requerente = cert.Subject.Replace("CN=", "").Replace("OU=", "").Replace("DC=", "").Replace("O=", "").Replace("C=", "");


            //ajusta a posição da assinatura
            float alturaPagina = pdfReader.GetPageSize(1).Height;
            float larguraPagina = pdfReader.GetPageSize(1).Right;

            int[] coordenadasAssinatura = new int[4];
            Rectangle rectangle = new Rectangle(0, 0, 0, 0);

            switch (posicaoAssinatura)
            {
                case 1:
                    rectangle = new Rectangle(0, alturaPagina - 5, larguraPagina / 3, alturaPagina - 55);
                    break;
                case 2:
                    rectangle = new Rectangle(larguraPagina / 3, alturaPagina - 5, larguraPagina / 3 * 2, alturaPagina - 55);
                    break;
                case 3:
                    rectangle = new Rectangle(larguraPagina / 3 * 2, alturaPagina - 5, larguraPagina, alturaPagina - 55);
                    break;
                case 4:
                    rectangle = new Rectangle(0, 10, larguraPagina / 3, 60);
                    break;
                case 5:
                    rectangle = new Rectangle(larguraPagina / 3, 10, larguraPagina / 3 * 2, 60);
                    break;
                case 6:
                    //rectangle = new Rectangle(350, 30, 550, 80);
                    rectangle = new Rectangle(larguraPagina / 3 * 2, 10, larguraPagina, 60);
                    break;
            }


            signatureAppearance.SetVisibleSignature(rectangle, 1, "Signature");
            signatureAppearance.Layer2Text = "Assinado de forma digital por " + requerente + Environment.NewLine + "Dados:" + DateTime.Now;



            //string pathImage = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assinatura.png");
            //var image = iTextSharp.text.Image.GetInstance(pathImage);

            //signatureAppearance.Image = iTextSharp.text.Image.GetInstance(pathImage);
            //signatureAppearance.ImageScale = 0.5F;
            //signatureAppearance.Image.Alignment = Element.ALIGN_JUSTIFIED;



            signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;

            MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
        }





        public String signPDF(String certName, String folder, int posicaoAssinatura)
        {
            String result = "";
            String output = folder + "\\Assinados\\";
            DirectoryInfo dSaida = new DirectoryInfo(output);
            dSaida.Create();

            X509Certificate2 cert = this.getCert(certName);

            DirectoryInfo d = new DirectoryInfo(folder);
            FileInfo[] Files = d.GetFiles("*.pdf");

            //Lista todos os arquivos PDF da pasta e executa o procedimento para cada um deles
            foreach (FileInfo file in Files)
            {
                try
                {
                    this.sign(cert, file.FullName, output + file.Name, posicaoAssinatura);
                    result += file.Name + " - Assinado" + Environment.NewLine;
                }
                catch
                {
                    result += file.Name + " - Erro" + Environment.NewLine; ;
                }
            }

            return result;
        }


        public X509Certificate2 getCert(String certName)
        {
            X509Certificate2Collection lcerts;
            X509Store lStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            // Abre o Store
            lStore.Open(OpenFlags.ReadOnly);

            // Lista os certificados
            lcerts = lStore.Certificates;

            foreach (X509Certificate2 cert in lcerts)
            {
                if (certName == cert.Subject)
                {

                    return cert;
                }
            }

            lStore.Close();
            return null;
        }


        public List<String> getCertList()
        {
            List<String> certList = new List<String>();

            X509Certificate2Collection lcerts;
            X509Store lStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            // Abre o Store
            lStore.Open(OpenFlags.ReadOnly);

            // Lista os certificados
            lcerts = lStore.Certificates;

            foreach (X509Certificate2 cert in lcerts)
            {



                if (cert.HasPrivateKey && cert.NotAfter > DateTime.Now && cert.NotBefore < DateTime.Now)
                {
                    try
                    {
                        RSACryptoServiceProvider rsacsp = (RSACryptoServiceProvider)cert.PrivateKey;
                        //if (rsacsp.CspKeyContainerInfo.Exportable) { 
                        certList.Add(cert.Subject);
                        //}
                    }
                    catch { }
                }
            }
            lStore.Close();
            return certList;
        }
    }
}
