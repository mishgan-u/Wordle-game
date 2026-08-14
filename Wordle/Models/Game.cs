using System;
using System.Collections.Generic;
using System.Text;

namespace Wordle.Models
{
    public class Game
    {
        public string TargetWord {  get; set; }=string.Empty;
        public List<Guess> Guesses { get; set; } = new();
        public int MaxAttemts { get; set; } = 6;
        public bool IsWon {  get; set; }
        public bool IsFinished { get; set; }


    }
}
