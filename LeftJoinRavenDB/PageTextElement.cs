using System;
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
                    WebtextCompare =(string)null
                });

            AddMap<PageTextElement>(compareElements => from compareElement in compareElements.Where(l=>l.Language=="sv")
                select new
                {
                    Page = compareElement.Page,
                    Token = compareElement.Token,
                    WebtextBase =(string)null,
                    WebtextCompare = compareElement.Webtext
                }
                );
            Reduce = results => from result in results
                group result by
                    new
                    {
                        Page = result.Page,
                        Token = result.Token
                    }
                into g
                select new
                {
                    
                    Page = g.Select(x => x.Page),
                    Token = g.Select(x => x.Token),
                    WebtextBase = g.Select(x => x.WebtextBase).Where(x => x != null).First(),
                    WebtextCompare = g.Select(x => x.WebtextCompare).Where(x => x != null).First(),
                };

            Index(x => x.WebtextBase, FieldIndexing.Analyzed);
        }
    }
}
//Url: "/indexes/LeftJoinPageTextTranslations"

//System.InvalidOperationException: Map functions defined as part of a multi map index must return identical types.
//Baseline map		: docs.PageTextElements.Select(baseElement => new {
//    Page = baseElement.Page,
//    Token = baseElement.Token,
//    WebtextBase = baseElement.Webtext
//})
//Non matching map	: docs.PageTextElements.Select(compareElement => new {
//    Page = compareElement.Page,
//    Token = compareElement.Token,
//    WebtextCompare = compareElement.Webtext
//})

//Common fields		: Page, Token, __document_id
//Missing fields		: WebtextBase
//Additional fields	: WebtextCompare
//   at Raven.Database.Linq.DynamicViewCompiler.HandleMapFunction(ConstructorDeclaration ctor, String map) in c:\Builds\RavenDB-Stable\Raven.Database\Linq\DynamicViewCompiler.cs:line 224
//   at Raven.Database.Linq.DynamicViewCompiler.TransformQueryToClass() in c:\Builds\RavenDB-Stable\Raven.Database\Linq\DynamicViewCompiler.cs:line 132
//   at Raven.Database.Linq.DynamicViewCompiler.GenerateInstance() in c:\Builds\RavenDB-Stable\Raven.Database\Linq\DynamicViewCompiler.cs:line 568
//   at Raven.Database.DocumentDatabase.PutIndex(String name, IndexDefinition definition) in c:\Builds\RavenDB-Stable\Raven.Database\DocumentDatabase.cs:line 1083
//   at Raven.Database.Server.Responders.Index.Put(IHttpContext context, String index) in c:\Builds\RavenDB-Stable\Raven.Database\Server\Responders\Index.cs:line 83
//   at Raven.Database.Server.HttpServer.DispatchRequest(IHttpContext ctx) in c:\Builds\RavenDB-Stable\Raven.Database\Server\HttpServer.cs:line 864
//   at Raven.Database.Server.HttpServer.HandleActualRequest(IHttpContext ctx) in c:\Builds\RavenDB-Stable\Raven.Database\Server\HttpServer.cs:line 609
