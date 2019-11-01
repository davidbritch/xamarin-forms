using Xamarin.Forms;

namespace BindFromControlTemplateToViewModel
{
    public class CardView : ContentView
    {
        public static readonly BindableProperty CardNameProperty = BindableProperty.Create(nameof(CardName), typeof(string), typeof(CardView), string.Empty);
        public static readonly BindableProperty CardDescriptionProperty = BindableProperty.Create(nameof(CardDescription), typeof(string), typeof(CardView), string.Empty);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CardView), Color.DarkGray);
        public static readonly BindableProperty CardColorProperty = BindableProperty.Create(nameof(CardColor), typeof(Color), typeof(CardView), Color.White);

        public string CardName
        {
            get => (string)GetValue(CardView.CardNameProperty);
            set => SetValue(CardView.CardNameProperty, value);
        }

        public string CardDescription
        {
            get => (string)GetValue(CardView.CardDescriptionProperty);
            set => SetValue(CardView.CardDescriptionProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(CardView.BorderColorProperty);
            set => SetValue(CardView.BorderColorProperty, value);
        }

        public Color CardColor
        {
            get => (Color)GetValue(CardView.CardColorProperty);
            set => SetValue(CardView.CardColorProperty, value);
        }
    }
}
