﻿
using BiliBiliAPI.ApiTools;
using BiliBiliAPI.Models;
using BiliBiliAPI.Models.Account;
using BiliBiliAPI.Tools;
using Newtonsoft.Json.Linq;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AsymmetricAlgorithm = PCLCrypto.AsymmetricAlgorithm;

namespace BiliBiliAPI.Account
{
    public class AccountPasswordLogin
    {

        private HttpTools HttpClient = new HttpTools();
        public async Task<ResultCode<PasswordLoginData>> LoginV3(string username, string password)
        {
            var pwd = await FormatPassword(password);
            string data = $"username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(pwd)}&gee_type=10";
            var results = await HttpClient.PostResults(Apis.LOGIN_PASSWD_LOGIN, data, HttpTools.ResponseEnum.App,null);
            return JsonConvert.ReadObject<PasswordLoginData>(results);
        }


        private async Task<string> FormatPassword(string passWord)
        {
            string base64String;
            try
            {
                string stringAsync = await HttpClient.PostResults(Apis.LOGIN_PASSWD_GET_KEY, string.Empty, HttpTools.ResponseEnum.App,null);
                var jObjects = JObject.Parse(stringAsync);
                string hash = jObjects["data"]["hash"].ToString();
                string key = jObjects["data"]["key"].ToString();
                string hashPass = string.Concat(hash, passWord);
                var collection = Regex.Match(key, "BEGIN PUBLIC KEY-----(?<key>[\\s\\S]+)-----END PUBLIC KEY");
                string publicKey = collection.Groups["key"].Value.Trim();
                byte[] numArray = Convert.FromBase64String(publicKey);
                var asymmetricKeyAlgorithmProvider = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
                var cryptographicKey = asymmetricKeyAlgorithmProvider.ImportPublicKey(numArray, 0);
                var buffer = WinRTCrypto.CryptographicEngine.Encrypt(cryptographicKey, Encoding.UTF8.GetBytes(hashPass), null);
                base64String = Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                base64String = passWord;
            }
            return base64String;
        }
    }
}
