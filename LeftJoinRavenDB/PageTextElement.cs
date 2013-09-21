using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace LeftJoinRavenDB
{
  public  class PageTextElement
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
        public PageTextElement BaseElement;
        public PageTextElement CompareElement;       
    }
    public class LeftJoinPageTextTranslations : AbstractMultiMapIndexCreationTask<ComparePageTextElement>
    {
        public LeftJoinPageTextTranslations()
        {
            AddMap<PageTextElement>(baseElements => from baseElement in baseElements
                select new
                {
                    BaseElement = new
                    {
                        Page = baseElement.Page,
                        Token = baseElement.Token,
                        Webtext = baseElement.Webtext,
                        Language = baseElement.Language,
                        Tranlator = baseElement.Translator,
                        CreationTime = baseElement.CreationTime
                    },
                    CompareElement = new
                    {
                        Page = String.Empty,
                        Token = String.Empty,
                        Webtext = String.Empty,
                        Language = String.Empty,
                        Tranlator = String.Empty,
                        CreationTime = String.Empty
                        //Page = baseElement.Page,
                        //Token = baseElement.Token,
                        //Webtext = baseElement.Webtext,
                        //Language = baseElement.Language,
                        //Tranlator = baseElement.Translator,
                        //CreationTime = baseElement.CreationTime
                    }
                });

            AddMap<PageTextElement>(compareElements => from compareElement in compareElements
                select new
                {
                    BaseElement = new
                    {
                        Page = String.Empty,
                        Token = String.Empty,
                        Webtext = String.Empty,
                        Language = String.Empty,
                        Tranlator = String.Empty,
                        CreationTime = String.Empty
                        //Page = compareElement.Page,
                        //Token = compareElement.Token,
                        //Webtext = compareElement.Webtext,
                        //Language = compareElement.Language,
                        //Tranlator = compareElement.Translator,
                        //CreationTime = compareElement.CreationTime
                    },
                    CompareElement = new
                    {
                        Page = compareElement.Page,
                        Token = compareElement.Token,
                        Webtext = compareElement.Webtext,
                        Language = compareElement.Language,
                        Tranlator = compareElement.Translator,
                        CreationTime = compareElement.CreationTime
                    }
                }
                );
            Reduce = results => from result in results
                                group result by new {Page=result.BaseElement.Page,Token=result.BaseElement.Token,Language=result.BaseElement.Language}
                                    into g
                                    select new
                                    {
                                      //  BaseElement = g.Select(x => new { Page = x.BaseElement.Page, Token = x.BaseElement, Language = x.BaseElement.Language }).Where(x => x != null).First(),
                                        BaseElement = g.Select(x => 
                                            new { Page = x.BaseElement.Page, Token = x.BaseElement.Token, Language = x.BaseElement.Language,x.BaseElement.Webtext,x.BaseElement.Translator }).First(x => x != null),
                                        CompareElement = g.Select(z => 
                                            new { Page = z.CompareElement.Page, Token = z.CompareElement.Token, Language = z.CompareElement.Language,z.BaseElement.Webtext, z.BaseElement.Translator }).First(z => z != null),                                    
                                    };
            /*
             * Url: "/indexes/LeftJoinPageTextTranslations"

System.InvalidOperationException: The result type is not consistent across map and reduce:
Common fields: 
Map only fields   : CreationTime, Language, Page, Token, Tranlator, Webtext
Reduce only fields: BaseElement, CompareElement

   at Raven.Database.Linq.DynamicViewCompiler.ValidateMapReduceFields(List`1 mapFields) in c:\Builds\RavenDB-Stable\Raven.Database\Linq\DynamicViewCompiler.cs:line 362
   at Raven.Database.Linq.DynamicViewCompiler.HandleReduceDefinition(ConstructorDeclaration ctor) in c:\Builds\RavenDB-Stable\Raven.Database\Linq\DynamicViewCompiler.cs:line 308
   at Raven.Database.Linq.DynamicViewCompiler.TransformQueryToClass() in c:\Builds\RavenDB-Stable\Raven.Database\Linq\DynamicViewCompiler.cs:line 139
   at Raven.Database.Linq.DynamicViewCompiler.GenerateInstance() in c:\Builds\RavenDB-Stable\Raven.Database\Linq\DynamicViewCompiler.cs:line 568
   at Raven.Database.Storage.IndexDefinitionStorage.AddAndCompileIndex(IndexDefinition indexDefinition) in c:\Builds\RavenDB-Stable\Raven.Database\Storage\IndexDefinitionStorage.cs:line 153
   at Raven.Database.Storage.IndexDefinitionStorage.CreateAndPersistIndex(IndexDefinition indexDefinition) in c:\Builds\RavenDB-Stable\Raven.Database\Storage\IndexDefinitionStorage.cs:line 138
   at Raven.Database.DocumentDatabase.PutIndex(String name, IndexDefinition definition) in c:\Builds\RavenDB-Stable\Raven.Database\DocumentDatabase.cs:line 1090
   at Raven.Database.Server.Responders.Index.Put(IHttpContext context, String index) in c:\Builds\RavenDB-Stable\Raven.Database\Server\Responders\Index.cs:line 83
   at Raven.Database.Server.HttpServer.DispatchRequest(IHttpContext ctx) in c:\Builds\RavenDB-Stable\Raven.Database\Server\HttpServer.cs:line 864
   at Raven.Database.Server.HttpServer.HandleActualRequest(IHttpContext ctx) in c:\Builds\RavenDB-Stable\Raven.Database\Server\HttpServer.cs:line 609

             * */

            Index(x => x.BaseElement.Token, FieldIndexing.Analyzed);
        }
    }
}
