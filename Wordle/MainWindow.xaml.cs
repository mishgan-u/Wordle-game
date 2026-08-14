using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wordle.Models;
using Wordle.Services;

namespace Wordle
{
    public partial class MainWindow : Window
    {
        private readonly List<TextBlock> _cells = new();

        private readonly GameService _gameService = new();

        private int _currentRow = 0;
        private int _currentColumn = 0;

        public MainWindow()
        {
            InitializeComponent();

            LoadCells();
            StartNewGame();
        }

        private void LoadCells()
        {
            foreach (var border in GameGrid.Children.OfType<Border>())
            {
                if (border.Child is TextBlock textBlock)
                {
                    _cells.Add(textBlock);
                }
            }
        }

        private void StartNewGame()
        {
            _gameService.StartNewGame();

            _currentRow = 0;
            _currentColumn = 0;

            foreach (TextBlock textBlock in _cells)
            {
                textBlock.Text = "";
                textBlock.Foreground = Brushes.Black;

                if (textBlock.Parent is Border border)
                {
                    border.Background = Brushes.Transparent;
                    border.BorderBrush = Brushes.Gray;
                }
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (_gameService.CurrentGame.IsFinished)
                return;

           
            if (_currentRow >= 6)
                return;

           
            if (e.Key == Key.Back)
            {
                DeleteLetter();
                return;
            }

           
            if (e.Key == Key.Enter)
            {
                SubmitWord();
                return;
            }

           
            string key = e.Key.ToString();

            if (key.Length == 1 && char.IsLetter(key[0]))
            {
                AddLetter(key.ToUpper());
            }
        }

        private void AddLetter(string letter)
        {
            
            if (_currentColumn >= 5)
                return;

            int index = (_currentRow * 5) + _currentColumn;

            _cells[index].Text = letter;

            _currentColumn++;
        }

        private void DeleteLetter()
        {
            if (_currentColumn <= 0)
                return;

            _currentColumn--;

            int index = (_currentRow * 5) + _currentColumn;

            _cells[index].Text = "";
        }

        private void SubmitWord()
        {
            
            if (_currentColumn < 5)
            {
                MessageBox.Show("Введите слово из 5 букв.");
                return;
            }

            string word = "";

           
            for (int column = 0; column < 5; column++)
            {
                int index = (_currentRow * 5) + column;

                word += _cells[index].Text;
            }

            try
            {
                
                Guess guess = _gameService.SumbitGuess(word);

               
                ColorCurrentRow(guess);

                
                if (_gameService.CurrentGame.IsWon)
                {
                    MessageBox.Show(
                        $"Вы победили!\nЗагаданное слово: {_gameService.CurrentGame.TargetWord}"
                    );

                    return;
                }

               
                if (_gameService.CurrentGame.IsFinished)
                {
                    MessageBox.Show(
                        $"Игра окончена!\nЗагаданное слово: {_gameService.CurrentGame.TargetWord}"
                    );

                    return;
                }

                
                _currentRow++;
                _currentColumn = 0;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ColorCurrentRow(Guess guess)
        {
            for (int column = 0; column < 5; column++)
            {
                int index = (_currentRow * 5) + column;

                TextBlock textBlock = _cells[index];

                if (textBlock.Parent is not Border border)
                    continue;

                switch (guess.Letters[column].Result)
                {
                    case LetterResult.Correct:
                        border.Background = Brushes.Green;
                        break;

                    case LetterResult.Present:
                        border.Background = Brushes.Goldenrod;
                        break;

                    case LetterResult.Absent:
                        border.Background = Brushes.Gray;
                        break;
                }

                textBlock.Foreground = Brushes.White;
            }
        }
    }
}