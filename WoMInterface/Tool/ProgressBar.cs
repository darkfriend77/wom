using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Tool
{
    public class ProgressBar
    {
        private int _lastOutputLength;
        private readonly int _maximumWidth;

        private bool initial = true;

        public ProgressBar(int maximumWidth)
        {
            _maximumWidth = maximumWidth;
            
        }

        public void Update(double percent)
        {
            if (initial)
            {
                Show(" [ ");
                initial = false;
            }

            // Remove the last state           
            string clear = string.Empty.PadRight(_lastOutputLength, '\b');

            Show(clear);

            // Generate new state           
            int width = (int)(percent / 100 * _maximumWidth);
            int fill = _maximumWidth - width;
            string output = string.Format("{0}{1} ] {2}%", string.Empty.PadLeft(width, '='), string.Empty.PadLeft(fill, ' '), percent.ToString("0.0"));
            Show(output);
            _lastOutputLength = output.Length;
        }

        private void Show(string value)
        {
            Console.Write(value);
        }
    }
}
