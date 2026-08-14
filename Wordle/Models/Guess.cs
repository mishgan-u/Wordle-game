using System;
using System.Collections.Generic;
using System.Text;

namespace Wordle.Models
{
    public class Guess
    {
        public string Word { get; set; }= string.Empty;
        public List<LetterGuess> Letters { get; set; } = new();

    }
}
