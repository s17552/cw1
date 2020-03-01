using System;

namespace Cw1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException();
            }
            Regex urlRegex = new Regex(@"http(s) ?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            MatchCollection urlMatches = urlRegex.Matches(args[0]);

            if (urlMatches.Count == 0)
            {
                throw new ArgumentException();
            }


            foreach(var a in args)
            {
                Console.WriteLine(a);
            }
            var emails = await GetEmails(args[0]);

            foreach (var a in emails)
            {
                Console.WriteLine(a);
            }
        }

        static async Task<HashSet<string>> GetEmails(string url)
        {
            HashSet<String> listOfEmails = new HashSet<String>();
            var httpClient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await httpClient.GetAsync(url);
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd w czasie pobierania strony" + e.Message);
            }


            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            RegexOptions.IgnoreCase);
            MatchCollection emailMatches = emailRegex.Matches(response.Content.ReadAsStringAsync().Result);

            if(emailMatches.Count == 0)
            {
                Console.WriteLine("Nie znaleziono adresów email");
            }
            else
            {
                foreach (var emailMatch in emailMatches)
                {
                    listOfEmails.Add(emailMatch.ToString());
                }
            }


            httpClient.Dispose();
            return listOfEmails;

        }
    }
}
