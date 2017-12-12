using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen
{
    public class TextSource
    {
        public TextSource()
        {
            Prefix = new List<string>();
            Sufix = new List<string>();
            Words = new List<string>();
            Middle = new List<string>();
            Options = new List<string>();
        }
        public List<string> Prefix { get; set; }
        public List<string> Sufix { get; set; }
        public List<string> Words { get; set; }
        public List<string> Middle { get; set; }

        public List<string> Options { get; set; }
    }

    
}
