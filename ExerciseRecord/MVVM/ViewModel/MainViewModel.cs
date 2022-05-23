using System;
using ExerciseRecord.Core;

namespace ExerciseRecord.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand TodayViewCommand { get; set; }
        public RelayCommand HighlightViewCommand { get; set; }
        public RelayCommand CalendarViewCommand { get; set; }


        public TodayViewModel TodayVM { get; set; }
        public HighlightViewModel HighlightVM { get; set; }
        public CalendarViewModel CalendarVM { get; set; }



        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            TodayVM = new TodayViewModel();
            HighlightVM = new HighlightViewModel();
            CalendarVM = new CalendarViewModel();

            CurrentView = TodayVM;

            TodayViewCommand = new RelayCommand(o =>
            {
                CurrentView = TodayVM;
            });

            HighlightViewCommand = new RelayCommand(o =>
            {
                CurrentView = HighlightVM;
            });

            CalendarViewCommand = new RelayCommand(o =>
            {
                CurrentView = CalendarVM;
            });
        }
    }
}
