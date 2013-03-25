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
    public class SearchIndex : IDisposable
    {
        private FSDirectory Dir;
        private StandardAnalyzer Analyzer;

        private IndexWriter Writer;

        private IndexReader Reader;
        private IndexSearcher Searcher;

        public SearchIndex()
        {
            Dir = FSDirectory.Open(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/LuceneIndex"));
            Analyzer = new StandardAnalyzer(Version.LUCENE_30);
            Writer = new IndexWriter(Dir, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

            Reader = IndexReader.Open(Dir, true);
            Searcher = new IndexSearcher(Dir, true);
        }

        public void AddOrUpdateIndex(Request r)
        {
            var searchQuery = new TermQuery(new Term("ID", r.ID.ToString()));
            Writer.DeleteDocuments(searchQuery);

            Document doc = ConvertRequest(r);

            Writer.AddDocument(doc);
        }

        public IList<int> Search(string queryStr)
        {
            var hitsLimit = 500;

            var fieldNames = Reader.GetFieldNames(IndexReader.FieldOption.ALL).ToArray();
            var parser = new MultiFieldQueryParser(Version.LUCENE_30, fieldNames, Analyzer);
            Query query = parser.Parse(queryStr);

            TopDocs docs = Searcher.Search(query, hitsLimit);

            List<int> requestIds = new List<int>();

            foreach (var scoreDoc in docs.ScoreDocs)
            {
                Document d = Searcher.Doc(scoreDoc.Doc);
            }

            return requestIds;
        }

        public Document ConvertRequest(Request r)
        {
            Document d = new Document();
            d.Add(new NumericField("ID", Field.Store.YES, true));
            d.Add(CreateField("", "Creator", r.Creator.UserName));
            if (r.Caller != null)
            {
                string prefix = "Caller";
                d.Add(CreateField(prefix, "Name", r.Caller.FirstName + " " + r.Caller.LastName));
                d.Add(CreateField(prefix, "Email", r.Caller.Email));
                d.Add(CreateField(prefix, "Region", r.Caller.Region.FullName));
                d.Add(CreateField(prefix, "Type", r.Caller.Type.FullName));
            }
            if (r.Patient != null)
            {
                string prefix = "Patient";
                d.Add(CreateField(prefix, "Name", r.Patient.FirstName + " " + r.Patient.LastName));
                d.Add(CreateField(prefix, "AgencyID", r.Patient.AgencyID));
            }
            int i = 0;
            StringBuilder keywords = new StringBuilder();
            foreach (Question q in r.Questions)
            {
                string prefix = "Question[" + i + "]";
                d.Add(CreateField(prefix, "QuestionText", q.QuestionText));
                d.Add(CreateField(prefix, "Response", q.Response));
                d.Add(CreateField(prefix, "Name", q.QuestionType.FullName));
                d.Add(CreateField(prefix, "Name", q.TumourGroup.FullName));
                foreach (Keyword k in q.Keywords)
                {
                    keywords.Append(k.KeywordText);
                    keywords.Append(" ");
                }
                i++;
            }
            d.Add(CreateField("", "Keywords", keywords.ToString()));
            return d;
        }

        public Field CreateField(string prefix, string prop, string value)
        {
            return new Field(prefix + prop, value, Field.Store.NO, Field.Index.ANALYZED);
        }

        public Term GetIndexTerm(Request r)
        {
            return new Term("ID", r.ID.ToString());
        }

        public bool ClearLuceneIndex()
        {
            try
            {
                Writer.DeleteAll();

                // close handles
                Analyzer.Close();
                Writer.Dispose();
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
            Writer.Dispose();
        }
    }
}