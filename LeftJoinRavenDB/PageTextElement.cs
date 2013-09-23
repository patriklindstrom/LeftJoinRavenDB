using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace LeftJoinRavenDB
{

    public class ComparePageTextElement
    {
        public string Page;
        public string Token;
        public string WebtextBase;
        public string WebtextCompare;
    }
    public class PageTextElement
    {
        public string Page;
        public string Token;
        public string Webtext;
        public string Language;
        public string Translator;
        public DateTime CreationTime;
        
    }


    public class ComparePageTextElementCount
    {
        public string Page;
        public string Token;
        public string Webtext;
        public string WebtextCompare;
        public int Count;
    }


    public class LeftJoinPageTextTranslationsCount : AbstractMultiMapIndexCreationTask<ComparePageTextElementCount>
    {
        public LeftJoinPageTextTranslationsCount()
        {
            AddMap<PageTextElement>(baseElements =>
                                    from baseElement in baseElements.Where(l => l.Language == "en")
                                    select
                                        new
                                        {
                                            baseElement.Page,
                                            baseElement.Token,
                                            baseElement.Webtext,
                                            WebtextCompare = (string)null,
                                            Count = 0
                                        });

            AddMap<PageTextElement>(compareElements =>
                                    from compareElement in compareElements.Where(l => l.Language == "sv")
                                    select
                                        new
                                        {
                                            compareElement.Page,
                                            compareElement.Token,
                                            Webtext = (string)null,
                                            WebtextCompare = compareElement.Webtext,
                                            Count = 1
                                        }
                );
            Reduce = results => from result in results
                                group result by
                                    new { result.Page, result.Token }
                                    into g
                                    select new
                                    {
                                        g.Key.Page,
                                        g.Key.Token,
                                        Webtext = g.Select(x => x.Webtext).FirstOrDefault(x => x != null),
                                        WebtextCompare = g.Select(x => x.WebtextCompare).FirstOrDefault(x => x != null),
                                        Count = g.Sum(x => x.Count)
                                    };
            Index(x => x.Webtext, FieldIndexing.Analyzed);
        }
    }
}
