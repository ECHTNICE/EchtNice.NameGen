using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen
{
    public class WordGenerator
    {
        TextSource source;
        List<string> filter;
        public WordGenerator(TextSource source)
        {
            this.source = source;
            this.filter = source.Options;
        }


        public List<string> Generate()
        {
            List<string> result = new List<string>();

            string separator = filter.Contains("SEP") ? " | " : "";

            result.Add(">>>PS");
            if (filter.Contains("PS"))
                foreach (var prefix in source.Prefix)
                    foreach (var sufix in source.Sufix)
                        result.Add(prefix + separator + sufix);

            result.Add(">>>PM");
            if (filter.Contains("PM"))
                foreach (var prefix in source.Prefix)
                    foreach (var middle in source.Middle)
                        result.Add(prefix + separator + middle);

            result.Add(">>>MS");
            if (filter.Contains("MS"))
                foreach (var middle in source.Middle)
                    foreach (var sufix in source.Sufix)
                        result.Add(middle + separator + sufix);


            result.Add(">>>PMS");
            if (filter.Contains("PMS"))
                foreach (var prefix in source.Prefix)
                    foreach (var middle in source.Middle)
                        foreach (var sufix in source.Sufix)
                            result.Add(prefix + separator + middle + separator + sufix);


            result.Add(">>>WW");
            if (filter.Contains("WW"))
                foreach (var w1 in source.Words)
                    foreach (var w2 in source.Words)
                            result.Add(w1 + separator + w2);

            result.Add(">>>PP");
            if (filter.Contains("PP"))
                foreach (var w1 in source.Prefix)
                    foreach (var w2 in source.Prefix)
                        result.Add(w1 + separator + w2);

            result.Add(">>>SS");
            if (filter.Contains("SS"))
                foreach (var w1 in source.Sufix)
                    foreach (var w2 in source.Sufix)
                        result.Add(w1 + separator + w2);

            result.Add(">>>MM");
            if (filter.Contains("MM"))
                foreach (var w1 in source.Middle)
                    foreach (var w2 in source.Middle)
                        result.Add(w1 + separator + w2);


            result.Add(">>>PW");
            if (filter.Contains("PW"))
                foreach (var prefix in source.Prefix)
                    foreach (var words in source.Words)
                        result.Add(prefix + separator + words);

            result.Add(">>>WS");
            if (filter.Contains("WS"))
                foreach (var words in source.Words)
                    foreach (var sufix in source.Sufix)
                        result.Add(words + separator + sufix);


            result.Add(">>>PWS");
            if (filter.Contains("PWS"))
                foreach (var prefix in source.Prefix)
                    foreach (var words in source.Words)
                        foreach (var sufix in source.Sufix)
                            result.Add(prefix + separator + words + separator + sufix);


            if (filter.Contains("UP"))
            {
                List<string> upresult = new List<string>();
                foreach (var item in result)
                {
                    upresult.Add(item.ToUpper());
                }
                return upresult;
            }
            
            return result;
        }
    }
}
