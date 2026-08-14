using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Wordle.Services
{
    public class WordService
    {
        private readonly List<string> _words;

        public WordService()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", "words.txt");

            _words=File.ReadAllLines(path)
                .Select(word=>word.Trim().ToUpper())
                .Where(word=>word.Length==5)
                .ToList();
        }

        public string GetRandomword()
        {
            Random r = new Random();

            int index = r.Next(_words.Count);

            return _words[index];
        }

        public bool IsValidWord(string word)
        {
            return _words.Contains(word.Trim().ToUpper());
        }
    }
}
