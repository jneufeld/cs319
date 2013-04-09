using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.IO;
using System.Text;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using DREAM.Models;
using DREAM.Helpers;
using Version = Lucene.Net.Util.Version;

namespace DREAM.Models
{
    public class SearchIndex<T, IndexDef> : IDisposable where IndexDef : IIndexDefinition<T>, new()
    {
        private FSDirectory Dir;
        private StandardAnalyzer Analyzer;

        private IndexDef Converter = new IndexDef();

        private Version LuceneVersion = Version.LUCENE_30;
        private int HitsLimit = 200;

        public static string DirPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\LuceneIndex";

        public SearchIndex()
        {
            Dir = FSDirectory.Open(new DirectoryInfo(DirPath));
            Analyzer = new StandardAnalyzer(LuceneVersion);
        }

        private void AddOrUpdateWithWriter(IndexWriter writer, T obj)
        {
            var idx = Converter.GetIndex(obj);
            var searchQuery = new TermQuery(idx);
            writer.DeleteDocuments(searchQuery);

            Document doc = Converter.Convert(obj);

            writer.AddDocument(doc);
            writer.Optimize();
        }

        public void AddOrUpdateIndex(T obj)
        {
            using (var writer = new IndexWriter(Dir, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                AddOrUpdateWithWriter(writer, obj);
            }
        }

        public void AddOrUpdateAll(IEnumerable<T> objs)
        {
            using (var writer = new IndexWriter(Dir, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var obj in objs)
                    AddOrUpdateWithWriter(writer, obj);
            }
        }

        public IList<int> Search(string queryStr)
        {
            using (var reader = IndexReader.Open(Dir, true))
            {
                using (var searcher = new IndexSearcher(reader))
                {
                    var fieldNames = reader.GetFieldNames(IndexReader.FieldOption.ALL).ToArray();
                    var parser = new MultiFieldQueryParser(LuceneVersion, fieldNames, Analyzer);
                    Query query = parser.Parse(queryStr);

                    TopDocs docs = searcher.Search(query, HitsLimit);

                    List<int> ids = new List<int>();

                    foreach (var scoreDoc in docs.ScoreDocs)
                    {
                        Document d = searcher.Doc(scoreDoc.Doc);
                        // FIXME Not generic in the slightest...
                        string val = d.Get("ID");
                        int id = 0;
                        Int32.TryParse(val, out id);
                        ids.Add(id);
                    }

                    return ids;
                }
            }
        }

        public bool ClearLuceneIndex()
        {
            try
            {
                using (var writer = new IndexWriter(Dir, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            Analyzer.Dispose();
        }
    }
}