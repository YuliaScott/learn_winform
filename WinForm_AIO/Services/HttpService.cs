using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using WinForm_AIO.Common;
using WinForm_AIO.Models;

namespace WinForm_AIO.Services
{
    public class HttpService
    {
        private static string httpHost = ConfigurationManager.AppSettings["apiHost"].ToString();

        public static Result GetAll(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                HttpClientHelper client = new HttpClientHelper(httpHost);
                string httpUrl = "api/Video/GetVideoList?pageindex=" + pageIndex + "&pagesize=" + pageSize + "";
                string responseStr = client.Get(httpUrl);
                var result = JsonConvert.DeserializeObject<Result>(responseStr);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取WebAPI数据_HttpService_GetAll,错误信息:"+ex.Message);
                return null;
            }
        }

        public static VideoEntity Get(string id)
        {
            try
            {
                HttpClientHelper client = new HttpClientHelper(httpHost);
                string httpUrl = "api/Video/GetVideoInfo?id=" + id;
                string responseStr = client.Get(httpUrl);
                var result = JsonConvert.DeserializeObject<VideoEntity>(responseStr);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取WebAPI数据_HttpService_Get,错误信息:" + ex.Message);
                return null;
            }
        }
    }
}
