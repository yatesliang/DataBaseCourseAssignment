using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace SceneView.Models
{
    public class MD5TransferAndVerify
    {

        public MD5 md5hash;

        public MD5TransferAndVerify()
        {
            md5hash = MD5.Create();
        }
        
        public string GetMD5Hash(string password)
        {
            byte[] data = md5hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public Boolean Verify(string md5password, string input)
        {
            string md5input = GetMD5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(md5input, md5password);

        }

    }
}