using System;
using System.Collections.Generic;
using System.Linq;
using WinForm_AIO.Models;

namespace WinForm_AIO.Services
{
    public static class MockData
    {
        private static List<VideoEntity> datas = new List<VideoEntity>();
        public static List<VideoEntity> Mock()
        {
            for (var i = 0; i < 300; i++)
            {
                var model = new VideoEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    VideoName = "测试" + i,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                    Path = "http://183.207.33.43:9011/clips.vorwaerts-gmbh.de/c3pr90ntc0td/big_buck_bunny.mp4",
                    Image = "https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201701%2F17%2F20170117140706_jAaWS.jpeg&refer=http%3A%2F%2Fb-ssl.duitang.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1633675061&t=4cf23f9509bcfde715bffd4b41761ce7",
                    Description = i + "实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的实打实大师法大幅度发大幅度的",
                    CodePath = "https://www.baidu.com/"
                };
                datas.Add(model);
            }
            return datas;
        }

        public static Result Get(int pageIndex, int pageSize)
        {
            var query = new List<VideoEntity>();

            datas = datas.Count > 0 ? datas : Mock();
            var totalPage = datas.Count % pageSize == 0 ? datas.Count / pageSize : datas.Count / pageSize + 1;
            if (pageIndex <= totalPage)
            {
                query = datas.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            var result = new Result
            {
                Data = query,
                TotalCount = datas.Count,
                TotalPage = totalPage,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return result;
        }

        public static VideoEntity GetThis(string id)
        {
            var result = datas.Where(s => s.ID.ToLower() == id.ToLower()).FirstOrDefault();
            return result;
        }
    }
}
