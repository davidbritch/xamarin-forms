namespace Imaging
{
    public class FrequencyRange
    {
        int min;
        int max;

        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public FrequencyRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
