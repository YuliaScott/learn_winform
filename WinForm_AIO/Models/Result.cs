using System.Collections.Generic;

namespace WinForm_AIO.Models
{
    public class Result
    {
        public List<VideoEntity> Data { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
