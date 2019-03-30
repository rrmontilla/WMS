using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WMS.Controller
{
   public class PasswordEncryptor
    {
       public string encrypt(string password)
       {
           string encrypt = "";
           

           MD5 md5 = new MD5CryptoServiceProvider();

           //computeHash

           md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

           //gethash result
           byte[] result = md5.Hash;

           StringBuilder strBuilder = new StringBuilder();
           for(int i =0; i < result.Length;i++)
           {
              //change into 2 decimal places
               strBuilder.Append(result[i].ToString("X2"));
            }
           encrypt = strBuilder.ToString();
           return encrypt;
       }
    }
}
