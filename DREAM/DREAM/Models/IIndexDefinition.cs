using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace DREAM.Models
{
    public interface IIndexDefinition<T>
    {
        Document Convert(T obj);
        Term GetIndex(T obj);
    }

    public class RequestIndexDefinition : IIndexDefinition<Request>
    {
        Document IIndexDefinition<Request>.Convert(Request r)
        {
            Document d = new Document();
            var idField = new NumericField("ID", Field.Store.YES, true);
            idField.SetIntValue(r.ID);
            d.Add(idField);
            AddField(d, "", "Creator", r.Creator);
            if (r.Caller != null)
            {
                string prefix = "Caller";
                AddField(d, prefix, "Name", r.Caller.FirstName + " " + r.Caller.LastName);
                AddField(d, prefix, "Email", r.Caller.Email);
                AddField(d, prefix, "Region", r.Caller.Region.FullName);
                AddField(d, prefix, "Type", r.Caller.Type.FullName);
            }
            if (r.Patient != null)
            {
                string prefix = "Patient";
                AddField(d, prefix, "Name", r.Patient.FirstName + " " + r.Patient.LastName);
                AddField(d, prefix, "AgencyID", r.Patient.AgencyID);
            }
            int i = 0;
            foreach (Question q in r.Questions)
            {
                string prefix = "Question" + i + "_";
                AddField(d, prefix, "QuestionText", q.QuestionText);
                AddField(d, prefix, "Response", q.Response);
                AddField(d, prefix, "QuestionType", q.QuestionType.FullName);
                AddField(d, prefix, "TumourGroup", q.TumourGroup.FullName);
                foreach (Keyword k in q.Keywords)
                {
                    var kw = CreateField("", "Keywords", k.KeywordText);
                    kw.Boost = 4.0f;
                    d.Add(kw);
                }
                i++;
            }
            return d;
        }

        Term IIndexDefinition<Request>.GetIndex(Request r)
        {
            return new Term("ID", r.ID.ToString());
        }

        private static void AddField(Document doc, string prefix, string prop, string value)
        {
            if (value != null)
                doc.Add(CreateField(prefix, prop, value));
        }

        private static Field CreateField(string prefix, string prop, string value)
        {
            return new Field(prefix + prop, value, Field.Store.NO, Field.Index.ANALYZED);
        }
    }
}