using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MVVMExample.DataModal
{
    public class JsonModalViewModel : ViewModelBase
    {
        public ObservableCollection<dynamic> JsonData { get; set; } = new();

        private LinkedList<dynamic> LinkedListData = new();

        private dynamic _selectedItem;
        public dynamic SelectedItem
        {
            get => _selectedItem;
            set => Set(nameof(SelectedItem), ref _selectedItem, value);
        }

        public ICommand LoadJsonCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }

        public JsonModalViewModel()
        {
            LoadJsonCommand = new RelayCommand(LoadJsonFile);
            MoveUpCommand = new RelayCommand(MoveUp, CanMoveUp);
            MoveDownCommand = new RelayCommand(MoveDown, CanMoveDown);
        }

        private void LoadJsonFile(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string jsonContent = File.ReadAllText(openFileDialog.FileName);
                var parsedData = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                JsonData.Clear();
                LinkedListData.Clear();


                foreach (var item in parsedData)
                {
                    JsonData.Add(item);
                    LinkedListData.AddLast(item);
                }
            }
        }

        private void MoveUp(object parameter)
        {
            if (SelectedItem == null) return;

            var currentNode = LinkedListData.Find(SelectedItem);
            if (currentNode?.Previous != null)
            {
                // Swap with the previous node
                var previousNode = currentNode.Previous;
                LinkedListData.Remove(currentNode);
                LinkedListData.AddBefore(previousNode, currentNode.Value);

                RefreshObservableCollection();
                SelectedItem = currentNode.Value;
            }
        }

        private void MoveDown(object parameter)
        {
            if (SelectedItem == null) return;

            var currentNode = LinkedListData.Find(SelectedItem);
            if (currentNode?.Next != null)
            {
                // Swap with the next node
                var nextNode = currentNode.Next;
                LinkedListData.Remove(currentNode);
                LinkedListData.AddAfter(nextNode, currentNode.Value);

                RefreshObservableCollection();
                SelectedItem = currentNode.Value;
            }
        }

        private bool CanMoveUp(object parameter) => SelectedItem != null && LinkedListData.Find(SelectedItem)?.Previous != null;

        private bool CanMoveDown(object parameter) => SelectedItem != null && LinkedListData.Find(SelectedItem)?.Next != null;

        private void RefreshObservableCollection()
        {
            JsonData.Clear();
            foreach (var item in LinkedListData)
            {
                JsonData.Add(item);
            }
        }
    }
}
