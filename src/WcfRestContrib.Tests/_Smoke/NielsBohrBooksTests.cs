using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using NUnit.Framework;
using WcfRestContrib.Net.Http;
using WcfRestContrib.Tests.TestData;
using WcfRestContrib.Tests.Utility;

namespace WcfRestContrib.Tests._Smoke
{
    /// <summary>
    /// Note that these smoke tests are dependent on the configuration of the
    /// NielsBohrLibrary web application (as well as its data!).
    /// </summary>
    [TestFixture]
    public class NielsBohrBooksTests
    {
        #region Test server management

        private TestWebServer _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = new TestWebServer(
                335,
                @"/",
                @"..\..\..\..\NielsBohrLibrary");
            _server.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }
        
        #endregion
       
        [Test, ExpectedException(typeof(WebException),
            MatchType = MessageMatch.Contains, ExpectedMessage = "401")]
        public void AddingOrModifyingABookRequiresAuthentication()
        {
            // Expecting to throw 401 - not authorized
            _server.Invoke(new TestRestRequest(Verbs.Put, Uris.FeynmanBookUri));
        }

        [Test]
        public void BooksHasMultipleBooks()
        {
            XDocument books = _server.GetXDocument("Books/");
            Assert.That(books.Element("Books").Elements("Book").Count(), Is.GreaterThan(1));
        }

        [Test]
        public void CanDeleteBookWhenAuthenticated()
        {
            // We delete the chemistry book so as not to disturb the other tests.
            // Hey, it's integration, forget perfection ;)
            HttpWebResponse response = _server.Invoke(
                new TestRestRequest(Verbs.Delete, Uris.TheresJustNoChemistryUri)
                    {
                        Credentials = Credentials.TonyClifton
                    }
                );

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CanGetSingleBook()
        {
            XDocument book = _server.GetXDocument(Uris.FeynmanBookUri);
            Assert.That(book.Element("Book").Element("Isbn").Value, Is.EqualTo(BookData.FeynmanBookIsbn));
        }

        [Test]
        public void CanGetSingleBookInJsonFormat()
        {
            string json = _server.GetResponse(
                new TestRestRequest(Verbs.Get, Uris.FeynmanBookUri)
                    {
                        Accept = "application/json"
                    }
                );
            Console.WriteLine(json);
            Assert.That(json, Text.Contains("{") & Text.Contains(BookData.FeynmanBookIsbn) & Text.Contains("}"));
        }

        [Test]
        public void CanModifyBookWhenAuthenticated()
        {
            HttpWebResponse response = _server.Invoke(
                new TestRestRequest(Verbs.Put, Uris.FeynmanBookUri)
                    {
                        Credentials = Credentials.TonyClifton,
                        Body = BookData.SampleBookUpdate
                    }
                );
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CanPostNewBook()
        {
            HttpWebResponse response = _server.Invoke(
                new TestRestRequest(Verbs.Post, Uris.NewBookUri)
                    {
                        Credentials = Credentials.TonyClifton,
                        Body = BookData.NewBook
                    });
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }
    }
}