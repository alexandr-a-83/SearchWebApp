using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SearchWebApp.Models;

namespace SearchWebApp.Services
{
    public class SearchEngine
    {
        public List<BookModel> _books;
        public int numHits;

        public SearchEngine(IEnumerable<BookModel> books)
        {
            _books = books.ToList();
        }

        public RAMDirectory CreateIndex(IEnumerable<BookModel> _books)
        {
            var directory = new RAMDirectory();

            using (var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
            using (var indexWriter = new IndexWriter(directory, analyzer, new IndexWriter.MaxFieldLength(1000)))
            {


                foreach (var book in _books)
                {
                    Document document = new Document();
                    document.Add(new Field("Id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("Author", book.Author, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("Name", book.Name, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("Description", book.Description, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("FullText", String.Format("{0} {1} {2} {3}", book.Id.ToString(), book.Author, book.Name, book.Description)
                            , Field.Store.YES, Field.Index.ANALYZED));

                    indexWriter.AddDocument(document);
                }

                indexWriter.Optimize();
                indexWriter.Flush(true, true, true);
            }

            return directory;
        }

        public IEnumerable<BookModel> Search(string searchValue)
        {
            var searchResult = new List<BookModel>();

            var indexDirectory = CreateIndex(_books);            

            using (var indexReader = IndexReader.Open(indexDirectory, true))
            using (var searcher = new IndexSearcher(indexReader))
            {
                using (Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
                {

                    try
                    {
                        var queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "FullText", analyzer);
                        var query = queryParser.Parse(searchValue);

                        var collector = TopScoreDocCollector.Create(numHits, true);

                        searcher.Search(query, collector);

                        var match = collector.TopDocs().ScoreDocs;

                        foreach (var item in match)
                        {
                            var id = item.Doc;
                            var doc = searcher.Doc(id);

                            var book = new BookModel();

                            book.Id = Int32.Parse(doc.GetField("Id").StringValue);
                            book.Author = doc.GetField("Author").StringValue;
                            book.Name = doc.GetField("Name").StringValue;
                            book.Description = doc.GetField("Description").StringValue;

                            searchResult.Add(book);
                        }
                    }
                    catch (Exception)
                    {
                        searchResult.Clear();
                    }

                }
            }

            return searchResult;
        }

    }
}
