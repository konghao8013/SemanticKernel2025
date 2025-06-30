using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK19Stuctured.Models
{
    public class MovieResult
    {
        public List<Movie> Movies { get; set; }
    }

    public class Movie
    {
        [Description("电影名称")]
        public string Title { get; set; }

        [Description("导演")]
        public string Director { get; set; }

        [Description("上映时间")]
        public string ReleaseDate { get; set; }

        [Description("评分")]
        public double Rating { get; set; }

        [Description("是否在流媒体上可用")]
        public bool IsAvailableOnStreaming { get; set; }

    }
}
