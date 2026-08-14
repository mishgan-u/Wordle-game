using System;
using System.Collections.Generic;
using System.Text;
using Wordle.Models;
namespace Wordle.Services
{
    public class GameService
    {
        private readonly WordService ? _wordService;
        private readonly GuessEvaluator? _guessEvaluator;

        public Game CurrentGame { get; private set; }
        
        public GameService()
        {
            _wordService = new WordService();
            _guessEvaluator = new GuessEvaluator();

            CurrentGame = new Game();
        }

        public void StartNewGame()
        {
            CurrentGame = new Game
            {
                TargetWord = _wordService.GetRandomword()
            };
        }

        public Guess SumbitGuess(string word)
        {
            if (CurrentGame.IsFinished)
            {
                throw new InvalidOperationException("The game is already finished.");
            }

            string normalizedWord = word.Trim().ToUpper();

            if (normalizedWord.Length != 5)
            {
                throw new ArgumentException("Word must contain exactly 5 letters.");
            }

            if (!_wordService.IsValidWord(normalizedWord))
            {
                throw new ArgumentException("Word not found.");
            }

            Guess guess = _guessEvaluator.Evaluate(
                CurrentGame.TargetWord,
                normalizedWord
            );

            
            CurrentGame.Guesses.Add(guess);

            
            if (normalizedWord == CurrentGame.TargetWord)
            {
                CurrentGame.IsWon = true;
                CurrentGame.IsFinished = true;
            }
            
            else if (CurrentGame.Guesses.Count >= CurrentGame.MaxAttempts)
            {
                CurrentGame.IsFinished = true;
            }

            return guess;
        }


    }
}
