using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;

namespace ExtendedFlexLayout
{
    public class MainPageViewModel
    {
        public ObservableCollection<Monkey> Monkeys { get; private set; }

        public ICommand AddMonkeysCommand => new Command(AddMonkeys);
        public ICommand RemoveMonkeyCommand => new Command(RemoveMonkey);

        public MainPageViewModel()
        {
            Monkeys = new ObservableCollection<Monkey>();

            Monkeys.Add(new Monkey
            {
                Name = "Seated Monkey",
                Description = "This monkey is laid back and relaxed, and likes to watch the world go by.",
                Trait1 = "• Doesn't make a lot of noise",
                Trait2 = "• Often smiles mysteriously",
                Trait3 = "• Sleeps sitting up",
                Image = "ExtendedFlexLayout.Images.SeatedMonkey.jpg",                 
            });
        }

        void AddMonkeys()
        {
            Monkeys.Add(new Monkey
            {
                Name = "Banana Monkey",
                Description = "Watch this monkey eat a giant banana.",
                Trait1 = "• More fun than a barrel of monkeys",
                Trait2 = "• Banana not included",
                Image = "ExtendedFlexLayout.Images.Banana.jpg"
            });
            Monkeys.Add(new Monkey
            {
                Name = "Face-Palm Monkey",
                Description = "This monkey reacts appropriately to ridiculous assertions and actions.",
                Trait1 = "• Cynical but not unfriendly",
                Trait2 = "• Seven varieties of grimaces",
                Trait3 = "• Doesn't laugh at your jokes",
                Image = "ExtendedFlexLayout.Images.FacePalm.jpg"
            });
        }

        void RemoveMonkey()
        {
            if (Monkeys.Any())
            {
                Monkeys.Remove(Monkeys.FirstOrDefault());
            }
        }
    }
}
