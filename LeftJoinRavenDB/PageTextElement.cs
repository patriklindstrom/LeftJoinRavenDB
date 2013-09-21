using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace LeftJoinRavenDB
{
    public class PageTextElement
    {
        public string Page;
        public string Token;
        public string Webtext;
        public string Language;
        public string Translator;
        public DateTime CreationTime;
        
    }

    public class ComparePageTextElement
    {
        public string Page;
        public string Token;
        public string WebtextBase;
        public string WebtextCompare;
    }
    public class ComparePageTextElementCount
    {
        public string Page;
        public string Token;
        public string Webtext;
        public int Count;
    }

    public class LeftJoinPageTextTranslations : AbstractMultiMapIndexCreationTask<ComparePageTextElement>
    {
        public LeftJoinPageTextTranslations()
        {
            AddMap<PageTextElement>(baseElements => from baseElement in baseElements.Where(l=>l.Language=="en")
                select new
                {

                    Page = baseElement.Page,
                    Token = baseElement.Token,
                    WebtextBase = baseElement.Webtext,
                    WebtextCompare = (string)null

                });

            AddMap<PageTextElement>(compareElements => from compareElement in compareElements.Where(l=>l.Language=="sv")
                select new
                {
                    Page = compareElement.Page,
                    Token = compareElement.Token,
                    WebtextBase = (string)null,
                    WebtextCompare = compareElement.Webtext
                }
                );
            Reduce = results => from result in results
                group result by
                    new
                    {
                    Page = result.Page, Token = result.Token
                    }
                into g
                select new
                {

                    Page = g.Select(x=>x.Page),
                    Token = g.Select(x => x.Token),
                    WebtextBase =    g.Select(x => x.WebtextBase).Where(x => x != null).First() ?? g.Select(x => x.WebtextCompare).Where(x => x != null).First(),
                    WebtextCompare = g.Select(x => x.WebtextBase).Where(x => x != null).First() ?? g.Select(x => x.WebtextCompare).Where(x => x != null).Last()

                };

            Index(x => x.WebtextBase, FieldIndexing.Analyzed);
        }
    }
    public class LeftJoinPageTextTranslationsCount : AbstractMultiMapIndexCreationTask<ComparePageTextElementCount>
    {
        public LeftJoinPageTextTranslationsCount()
        {
            AddMap<PageTextElement>(baseElements => 
                from baseElement in baseElements.Where(l => l.Language == "en")
                select new { Page = baseElement.Page, Token = baseElement.Token, baseElement.Webtext,WebtextCompare=(string)null, Count = 0 });

            AddMap<PageTextElement>(compareElements => 
                from compareElement in compareElements.Where(l => l.Language == "sv")
                select new { Page = compareElement.Page, Token = compareElement.Token, Webtext = (string)null,WebtextCompare=compareElement.Webtext, Count = 1 }
                );
            Reduce = results => from result in results
                                group result by
                                    new{Page = result.Page,Token = result.Token}
                                    into g
                                    select new
                                    {
                                        Page = g.Select(x => x.Page).Where(x => x != null).First(),
                                        Token = g.Select(x => x.Token).Where(x => x != null).First(),
                                        Webtext = g.Select(x => x.Webtext).Where(x => x != null).First(),
                                        WebtextCompare=g.Select(x => x.Webtext).Where(x => x != null).Last(),
                                        Count = g.Sum( x => x.Count)
                                    };
            Index(x => x.Page, FieldIndexing.Analyzed);
        }
    }
}
