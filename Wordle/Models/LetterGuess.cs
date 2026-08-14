using System;
using System.Collections.Generic;
using System.Text;

namespace Wordle.Models
{
    public class LetterGuess
    {
        public char Letter {  get; set; }
        public LetterResult Result { get; set; }
    }
}
