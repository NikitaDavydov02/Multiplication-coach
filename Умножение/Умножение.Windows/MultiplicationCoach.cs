using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace Умножение
{
    class MultiplicationCoach:INotifyPropertyChanged
    {
        DispatcherTimer timerForText;
        //Binding properties
        public string Example { get; private set; }
        public string Answer { get; set; }
        public string Result { get; private set; }
        public string Statistics { get; private set; }
        public string Accuracy { get; private set; }

        //Fields for statistics
        private int total = 0;
        private int correct = 0;
        private int uncorrect = 0;
        private int currentAccuracy = 0;
        public int accuracy { get; private set; }

        //Fields for internal using
        Random random = new Random();
        private int firstMultiplier;
        private int secondMultiplier;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChangedEvent = PropertyChanged;
            if (propertyChangedEvent != null)
                propertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
        }

        //Methods
        public bool CheckTheAnswer(int answer)
        {
            int correctAnswer = firstMultiplier * secondMultiplier;
            if (answer == correctAnswer)
                return true;
            else
                return false;
        }
        public void StartTheGame()
        {
            timerForText = new DispatcherTimer();
            timerForText.Interval = TimeSpan.FromMilliseconds(10);
            timerForText.Tick += TimerForText_Tick;
            Result = "";
            Statistics = "";
            Answer = "";
            Example = "";
            Accuracy = "";
            accuracy = 0;
            total = 0;
            correct = 0;
            uncorrect = 0;
            OnPropertyChanged("Result");
            OnPropertyChanged("Statistics");
            OnPropertyChanged("Answer");
            OnPropertyChanged("Example");
            OnPropertyChanged("Accuracy");
            OnPropertyChanged("accuracy");
            NewExample();
        }

        private void TimerForText_Tick(object sender, object e)
        {
            if (accuracy >= currentAccuracy)
            {
                Accuracy = "Точность ответа: " + currentAccuracy + " %";
                OnPropertyChanged("Accuracy");
                currentAccuracy++;
            }
            else
            {
                timerForText.Stop();
            }
        }

        private void NewExample()
        {
            firstMultiplier = random.Next(1, 100);
            secondMultiplier = random.Next(1, 100);
            Example = firstMultiplier + " X " + secondMultiplier + " =";
            OnPropertyChanged("Example");
        }
        public bool UpdateStatistics(bool correctnessOfTheAnswer)
        {
            total++;
            if (correctnessOfTheAnswer)
            {
                correct++;
                Result = "Ответ верный!";
                OnPropertyChanged("Result");
            }
            else
            {
                uncorrect++;
                Result = "Ошибка!";
                OnPropertyChanged("Result");
            }
            accuracy = 100 * correct / total;
            Answer = "";
            OnPropertyChanged("Answer");
            if (total >=5)
            {
                EndTheGame();
                return true;
            }
            NewExample();
            return false;
        }
        private void EndTheGame()
        {
            Answer = "";
            Example = "";
            Result = "";
            Statistics = "Тренировка окончена!" + Environment.NewLine;
            Statistics += "Правильно: " + correct + Environment.NewLine;
            Statistics += "Неправильно: " + uncorrect + Environment.NewLine;
            OnPropertyChanged("Result");
            OnPropertyChanged("Statistics");
            OnPropertyChanged("Answer");
            OnPropertyChanged("Example");
            timerForText.Start();
        }
    }
}
