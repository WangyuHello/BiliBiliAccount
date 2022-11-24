﻿using BiliBiliAPI.Models;
using BiliBiliAPI.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliBiliAPI.Search
{

    public class DefaultSearch
    {
        MyHttpClient HttpClient = new MyHttpClient();
        public async Task<ResultCode<SearchDefaultData>> GetDefault()
        {
            string url = "https://api.bilibili.com/x/web-interface/search/default";
            return JsonConvert.ReadObject<SearchDefaultData>(await HttpClient.GetResults(url));
        }


    }
}
