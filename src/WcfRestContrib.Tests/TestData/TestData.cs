using System.Net;

namespace WcfRestContrib.Tests.TestData
{
    /// <summary>
    /// Some general data to abuse in the tests
    /// </summary>
    internal static class Uris
    {
        internal const string TheresJustNoChemistryUri = "Books/0471775231";
        internal const string FeynmanBookUri = "Books/" + BookData.FeynmanBookIsbn;
        internal const string NewBookUri = "Books/?redirecturl=Books/";
    }

    internal static class BookData
    {
        internal const string FeynmanBookIsbn = "0691125759";

        internal const string SampleBookUpdate =
            @"<Book><Author>Richard P. Feynman</Author><Description>Princeton University Press (2006), Paperback, 192 pages</Description><Isbn>0691125759</Isbn><Language>English</Language><Published>2006</Published><Summary>QED: The Strange Theory of Light and Matter (Princeton Science Library) by Richard P. Feynman (2006)</Summary><Title>QED: The Strange Theory of Light and Matter (Princeton Science Library)</Title></Book>";
        internal const string NewBook =
            @"<Book><Author>Russell A. Garner</Author><Description>Zephyros Systems (2009), 1 pager</Description><Isbn>123456789</Isbn><Language>English</Language><Published>2009</Published><Summary>I got a bit bored after 100 words</Summary><Title>The Shortest Textbook I Could Get Away With</Title></Book>";
    }

    internal static class Credentials
    {
        internal static ICredentials TonyClifton = new NetworkCredential("tony", "clifton");
    }
}