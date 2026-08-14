using System;
using System.Collections.Generic;
using System.Text;
using Wordle.Models;
namespace Wordle.Services
{
    public class GuessEvaluator
    {
        public Guess Evaluate(string targetWord, string guessedWord)
        {
            targetWord = targetWord.Trim().ToUpper();
            guessedWord = guessedWord.Trim().ToUpper();

            var guess = new Guess
            {
                Word = guessedWord
            };
            
            var results= new LetterResult[guessedWord.Length];
            var usedTargetLetters= new bool[targetWord.Length];

            for (int i = 0; i< guessedWord.Length; i++)
            {
                if (guessedWord[i] == targetWord[i])
                {
                    results[i] = LetterResult.Correct;
                    usedTargetLetters[i] = true;
                }
            }

            for(int i = 0;i< guessedWord.Length; i++)
            {
                if (results[i] == LetterResult.Correct)
                    continue;

                bool found = false;

                for (int j=0; j<targetWord.Length; j++)
                {
                    if (!usedTargetLetters[j] && guessedWord[i] == targetWord[j])
                    {
                        results[i] = LetterResult.Present;
                        usedTargetLetters[j] = true;
                        found = true;
                        break;
                    }
                }

                if (!found)
                    results[i] = LetterResult.Absent;
            }

            for (int i = 0; i < guessedWord.Length; i++)
            {
                guess.Letters.Add(new LetterGuess
                {
                    Letter = guessedWord[i],
                    Result = results[i]
                });
            }

            return guess;
        }
        
    }
}
