namespace AoC2019
{
    public class NetworkIntcode : Intcode
    {
        public NetworkIntcode(string textMemory) : base(textMemory)
        {
        }

        protected override long GetInput()
        {
            if (InputQueue.Count > 0)
                return base.GetInput();
            else
            {
                return -1;
            }
        }
    }
}
