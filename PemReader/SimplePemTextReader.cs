// https://gist.githubusercontent.com/djeikyb/fb7fe2fe18f334fa82d803aebbc7d740/raw/6c2d2a7c032017fc598c9fefcac8f318ce487e44/read-pem-and-key-with-or-without-tags.cs

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PemReader;

public class SimplePemTextReader
{
    public static (X509Certificate2, RSA) Read(string publicCert, string privateKeyPemTaggedOrNot)
    {
        var rsa = RSA.Create();
        if (privateKeyPemTaggedOrNot.StartsWith("-----BEGIN RSA PRIVATE KEY-----"))
        {
            rsa.ImportFromPem(privateKeyPemTaggedOrNot);
        }
        else
        {
            var key = Convert.FromBase64String(privateKeyPemTaggedOrNot);
            rsa.ImportRSAPrivateKey(key, out _);
        }

        X509Certificate2 cert;
        if (publicCert.StartsWith("-----BEGIN CERTIFICATE-----"))
        {
            cert = X509Certificate2.CreateFromPem(publicCert, rsa.ExportRSAPrivateKeyPem());
        }
        else
        {
            var bytes = Convert.FromBase64String(publicCert);
            cert = new X509Certificate2(bytes);
            cert = cert.CopyWithPrivateKey(rsa);
        }

        return (cert, rsa);
    }
}